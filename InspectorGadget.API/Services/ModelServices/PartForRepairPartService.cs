using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class PartForRepairPartService : EntityService<PartForRepairPart, PartForRepairPartDto>
{
    public PartForRepairPartService(PartForRepairPartRepository repository) : base(repository)
    {
    }
}