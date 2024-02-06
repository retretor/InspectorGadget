using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RepairTypesListService : EntityService<RepairTypesList, RepairTypesListDto>
{
    public RepairTypesListService(RepairTypesListRepository repository) : base(repository)
    {
    }
}