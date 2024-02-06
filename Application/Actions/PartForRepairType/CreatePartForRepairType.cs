using MediatR;

namespace Application.Actions.PartForRepairType;

public class CreatePartForRepairType : IRequest<int>
{
    public int PartCount { get; init; }
    public int RepairTypeForDeviceId { get; init; }
    public int RepairPartId { get; init; }
}