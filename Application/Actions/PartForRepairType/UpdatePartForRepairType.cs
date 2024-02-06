using MediatR;

namespace Application.Actions.PartForRepairType;

public class UpdatePartForRepairType : IRequest
{
    public int Id { get; init; }
    public int PartCount { get; init; }
    public int RepairTypeForDeviceId { get; init; }
    public int RepairPartId { get; init; }
}