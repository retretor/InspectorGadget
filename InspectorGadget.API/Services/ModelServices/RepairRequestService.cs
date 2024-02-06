using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RepairRequestService : EntityService<RepairRequest, RepairRequestDto>
{
    public RepairRequestService(RepairRequestRepository repository) : base(repository)
    {
    }
}