using InspectorGadget.Db.ModelRepositories;

namespace InspectorGadget.Services.ModelServices;

public abstract class EntityService<T, TD> where T : class where TD : struct
{
    protected readonly BaseRepository<T> Repository;

    protected EntityService(BaseRepository<T> repository)
    {
        Repository = repository;
    }

    public async Task<IEnumerable<T>?> Get()
    {
        var entities = await Repository.GetAllAsync();
        return entities;
    }

    public async Task<T?> Get(int id)
    {
        var entity = await Repository.GetAsync(id);
        return entity;
    }

    public async Task<T?> Update(int id, TD dto)
    {
        var updatedEntity = await Repository.UpdateAsync(id, dto);
        return updatedEntity;
    }

    public async Task<T?> Create(TD dto)
    {
        var createdEntity = await Repository.CreateAsync(dto);
        return createdEntity;
    }

    public async Task Delete(int id)
    {
        await Repository.DeleteAsync(id);
    }
}