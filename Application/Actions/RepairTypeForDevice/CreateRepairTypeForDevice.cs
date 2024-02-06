using MediatR;

namespace Application.Actions.RepairTypeForDevice;

public class CreateRepairTypeForDevice : IRequest<int>
{
    public float Cost { get; init; }
    public int Time { get; init; }
    public int RepairTypeId { get; init; }
    public int DeviceId { get; init; }
}