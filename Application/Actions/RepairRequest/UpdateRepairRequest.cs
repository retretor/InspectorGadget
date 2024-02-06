using MediatR;

namespace Application.Actions.RepairRequest;

public class UpdateRepairRequest : IRequest
{
    public int Id { get; init; }
    public int DeviceId { get; init; }
    public int ClientId { get; init; }
    public int EmployeeId { get; init; }
    public string SerialNumber { get; init; } = null!;
}