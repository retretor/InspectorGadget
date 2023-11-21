using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RepairPartService : IEntityService<RepairPart, RepairPartDto>
{
    public IDbRepository Repository { get; } = new RepairPartRepository();
}