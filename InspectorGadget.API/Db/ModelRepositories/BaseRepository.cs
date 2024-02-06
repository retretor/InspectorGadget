﻿using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Db.ModelRepositories;

public abstract class BaseRepository<T> where T : class
{
    protected readonly InspectorGadgetContext Context;

    protected BaseRepository()
    {
        Context = new InspectorGadgetContext(new DbContextOptions<InspectorGadgetContext>());
        Context.Database.EnsureCreated();
    }
    
    public async Task<IEnumerable<T>?> GetAllAsync() 
    {
        var entities = await Context.Set<T>().ToListAsync();
        if (entities.Count == 0)
        {
            Console.WriteLine($"No {typeof(T).Name} found");
            return null;
        }

        Console.WriteLine($"{typeof(T).Name} count: {entities.Count}");
        return entities;
    }
    
    public async Task<T?> GetAsync(int id) 
    {
        var entities = await Context.Set<T>().ToListAsync();
        var entity = entities.FirstOrDefault(e => e.GetType().GetProperty("Id")?.GetValue(e)?.Equals(id) == true);
        if (entity == null)
        {
            Console.WriteLine($"{typeof(T).Name} not found: {id}");
            return null;
        }
        
        return entity;
    }
    
    public async Task<T?> UpdateAsync(int id, object dto) 
    {
        var entities = await Context.Set<T>().ToListAsync();
        var entityToUpdate = entities.FirstOrDefault(e => e.GetType().GetProperty("Id")?.GetValue(e)?.Equals(id) == true);
        if (entityToUpdate == null)
        {
            Console.WriteLine($"{typeof(T).Name} not found: {id}");
            return null;
        }

        var dtoProperties = dto.GetType().GetProperties();
        foreach (var property in dtoProperties)
        {
            var entityProperty = entityToUpdate.GetType().GetProperty(property.Name);
            if (entityProperty != null && entityProperty.CanWrite)
            {
                entityProperty.SetValue(entityToUpdate, property.GetValue(dto));
            }
        }

        await Context.SaveChangesAsync();
        return entityToUpdate;
    }
    
    public virtual async Task<T?> CreateAsync(object dto) 
    {
        var entities = await Context.Set<T>().ToListAsync();
        var id = (int)(dto.GetType().GetProperty("Id")?.GetValue(dto) ?? 0);
        if (entities.Any(e => e.GetType().GetProperty("Id")?.GetValue(e)?.Equals(id) == true))
        {
            Console.WriteLine($"{typeof(T).Name} already exists: {id}");
            return null;
        }

        var entityType = typeof(T);
        var entity = Activator.CreateInstance(entityType);
        if (entity == null)
        {
            Console.WriteLine($"Failed to create {typeof(T).Name}");
            return null;
        }
        var dtoProperties = dto.GetType().GetProperties();
        foreach (var property in dtoProperties)
        {
            var entityProperty = entity.GetType().GetProperty(property.Name);
            if (entityProperty != null && entityProperty.CanWrite)
            {
                entityProperty.SetValue(entity, property.GetValue(dto));
            }
        }
        Context.Set<T>().Add((T)entity);
        await Context.SaveChangesAsync();
        return (T)entity;
    }

    public async Task DeleteAsync(int id) 
    {
        var entities = await Context.Set<T>().ToListAsync();
        var entityToDelete = entities.FirstOrDefault(e => e.GetType().GetProperty("Id")?.GetValue(e)?.Equals(id) == true);
        if (entityToDelete == null)
        {
            Console.WriteLine($"{typeof(T).Name} not found: {id}");
            return;
        }

        Context.Set<T>().Remove(entityToDelete);
        await Context.SaveChangesAsync();
    }
}