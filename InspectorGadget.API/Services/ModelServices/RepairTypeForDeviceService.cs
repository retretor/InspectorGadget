using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RepairTypeForDeviceService : EntityService<RepairTypeForDevice, RepairTypeForDeviceDto>
{
    public RepairTypeForDeviceService(RepairTypeForDeviceRepository repository) : base(repository)
    {
    }
}