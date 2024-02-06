using MediatR;

namespace Application.Actions.RepairTypesList;

public class UpdateRepairTypesList : IRequest
{
    public int Id { get; init; }
    public int RepairTypeForDeviceId { get; init; }
    public int RepairRequestId { get; init; }
}