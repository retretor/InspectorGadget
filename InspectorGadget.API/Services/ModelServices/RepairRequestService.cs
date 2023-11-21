using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RepairRequestService : IEntityService<RepairRequest, RepairRequestDto>
{
    public IDbRepository Repository { get; } = new RepairRequestRepository();
}