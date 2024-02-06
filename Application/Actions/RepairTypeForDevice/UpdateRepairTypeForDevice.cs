using MediatR;

namespace Application.Actions.RepairTypeForDevice;

public class UpdateRepairTypeForDevice : IRequest
{
    public int Id { get; init; }
    public float Cost { get; init; }
    public int Time { get; init; }
    public int RepairTypeId { get; init; }
    public int DeviceId { get; init; }
}