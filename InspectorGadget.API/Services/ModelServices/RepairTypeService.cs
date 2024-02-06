using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RepairTypeService : EntityService<RepairType, RepairTypeDto>
{
    public RepairTypeService(RepairTypeRepository repository) : base(repository)
    {
    }
}