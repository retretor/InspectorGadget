using InspectorGadget.DTOs;
using InspectorGadget.Models;
using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Db.ModelRepositories;

public class DbUserRepository : IDbRepository
{
    public DbUserRepository()
    {
        Context = new InspectorGadgetContext(new DbContextOptions<InspectorGadgetContext>());
        Context.Database.EnsureCreated();
    }

    public InspectorGadgetContext Context { get; init; }
    
    public async Task<DbUser?> CreateAsync(DbUserAuthDto dto)
    {
        var entities = await Context.DbUsers.ToListAsync();
        var id = dto.Id;
        if (entities.Any(e => e.Id == id))
        {
            Console.WriteLine($"DbUser already exists: {id}");
            return null;
        }
        
        var newEntity = new DbUser(dto.FirstName, dto.SecondName, dto.TelephoneNumber, dto.Login, dto.PasswordHash, dto.Role);
        Context.DbUsers.Add(newEntity);
        await Context.SaveChangesAsync();
        return newEntity;
    }
}