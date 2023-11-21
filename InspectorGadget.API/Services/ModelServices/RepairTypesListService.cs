using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RepairTypesListService : IEntityService<RepairTypesList, RepairTypesListDto>
{
    public IDbRepository Repository { get; } = new RepairTypesListRepository();
}