using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RepairTypeForDeviceService : IEntityService<RepairTypeForDevice, RepairTypeForDeviceDto>
{
    public IDbRepository Repository { get; } = new RepairTypeForDeviceRepository();
}