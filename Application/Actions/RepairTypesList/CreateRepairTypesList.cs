using MediatR;

namespace Application.Actions.RepairTypesList;

public class CreateRepairTypesList : IRequest<int>
{
    public int RepairTypeForDeviceId { get; init; }
    public int RepairRequestId { get; init; }
}