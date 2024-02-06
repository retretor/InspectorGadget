using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RepairPartService : EntityService<RepairPart, RepairPartDto>
{
    public RepairPartService(RepairPartRepository repository) : base(repository)
    {
    }
}