using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RepairTypeService : IEntityService<RepairType, RepairTypeDto>
{
    public IDbRepository Repository { get; } = new RepairTypeRepository();
}