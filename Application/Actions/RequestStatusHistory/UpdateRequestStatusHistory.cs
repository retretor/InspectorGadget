using MediatR;

namespace Application.Actions.RequestStatusHistory;

public class UpdateRequestStatusHistory : IRequest
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public int RepairRequestId { get; init; }
    public string RequestStatus { get; init; } = null!;
}