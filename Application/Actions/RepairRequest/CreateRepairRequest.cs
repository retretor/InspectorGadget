using MediatR;

namespace Application.Actions.RepairRequest;

public class CreateRepairRequest : IRequest<int>
{
    public int DeviceId { get; init; }
    public int ClientId { get; init; }
    public int EmployeeId { get; init; }
    public string SerialNumber { get; init; } = null!;
}