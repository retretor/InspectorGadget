using InspectorGadget.Db;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services;

public class DeviceService
{
    private readonly DbRepository _repository = new();

    public async Task<IEnumerable<Device>?> Get()
    {
        var entities = await _repository.GetAllAsync<Device>();
        return entities;
    }

    public async Task<Device?> Get(int id)
    {
        var entity = await _repository.GetAsync<Device>(id);
        return entity;
    }

    public async Task<Device?> Update(int id, DeviceDto dto)
    {
        var updatedEntity = await _repository.UpdateAsync<Device>(id, dto);
        return updatedEntity;
    }

    public async Task<Device?> Create(DeviceDto dto)
    {
        var createdEntity = await _repository.CreateAsync<Device>(dto);
        return createdEntity;
    }

    public async Task Delete(int id)
    {
        await _repository.DeleteAsync<Device>(id);
    }
}