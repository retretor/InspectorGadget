using InspectorGadget.Db.ModelRepositories;

namespace InspectorGadget.Services.ModelServices;

public interface IEntityService<T, in TD> where T : class where TD : struct
{
    protected IDbRepository Repository { get; }

    public async Task<IEnumerable<T>?> Get()
    {
        var entities = await Repository.GetAllAsync<T>();
        return entities;
    }

    public async Task<T?> Get(int id)
    {
        var entity = await Repository.GetAsync<T>(id);
        return entity;
    }

    public async Task<T?> Update(int id, TD dto)
    {
        var updatedEntity = await Repository.UpdateAsync<T>(id, dto);
        return updatedEntity;
    }

    public async Task<T?> Create(TD dto)
    {
        var createdEntity = await Repository.CreateAsync<T>(dto);
        return createdEntity;
    }

    public async Task Delete(int id)
    {
        await Repository.DeleteAsync<T>(id);
    }
}