using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class PartForRepairPartService : IEntityService<PartForRepairPart, PartForRepairPartDto>
{
    public IDbRepository Repository { get; } = new PartForRepairPartRepository();
}