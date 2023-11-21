using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class AllowedRepairTypesForEmployeeService
{
    private readonly IDbRepository _repository = new AllowedRepairTypesForEmployeeRepository();

    public async Task<IEnumerable<AllowedRepairTypesForEmployee>?> Get()
    {
        var entities = await _repository.GetAllAsync<AllowedRepairTypesForEmployee>();
        return entities;
    }

    public async Task<AllowedRepairTypesForEmployee?> Get(int id)
    {
        var entity = await _repository.GetAsync<AllowedRepairTypesForEmployee>(id);
        return entity;
    }

    public async Task<AllowedRepairTypesForEmployee?> Update(int id, AllowedRepairTypesForEmployeeDto dto)
    {
        var updatedEntity = await _repository.UpdateAsync<AllowedRepairTypesForEmployee>(id, dto);
        return updatedEntity;
    }

    public async Task<AllowedRepairTypesForEmployee?> Create(AllowedRepairTypesForEmployeeDto dto)
    {
        var createdEntity = await _repository.CreateAsync<AllowedRepairTypesForEmployee>(dto);
        return createdEntity;
    }

    public async Task Delete(int id)
    {
        await _repository.DeleteAsync<AllowedRepairTypesForEmployee>(id);
    }
}