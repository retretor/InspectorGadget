using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class DeviceService : EntityService<Device, DeviceDto>
{
    public DeviceService(DeviceRepository repository) : base(repository)
    {
    }

    public async Task<Device?> UpdateDevice(int id, DeviceDto dto)
    {
        var allowedTypes = Enum.GetNames(typeof(AllowedDeviceType));
        dto.Type = dto.Type.ToUpper();
        if (!allowedTypes.Contains(dto.Type.ToUpper()))
        {
            return null;
        }

        var updatedEntity = await Update(id, dto);
        return updatedEntity;
    }

    public async Task<Device?> CreateDevice(DeviceDto dto)
    {
        var allowedTypes = Enum.GetNames(typeof(AllowedDeviceType));
        dto.Type = dto.Type.ToUpper();
        if (!allowedTypes.Contains(dto.Type.ToUpper()))
        {
            return null;
        }
        
        if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Type) || string.IsNullOrEmpty(dto.Brand) ||
            string.IsNullOrEmpty(dto.Series) || string.IsNullOrEmpty(dto.Manufacturer))
        {
            return null;
        }

        var createdEntity = await Create(dto);
        return createdEntity;
    }
}