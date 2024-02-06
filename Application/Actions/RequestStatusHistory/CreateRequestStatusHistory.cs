using MediatR;

namespace Application.Actions.RequestStatusHistory;

public class CreateRequestStatusHistory : IRequest<int>
{
    public DateTime Date { get; init; }
    public int RepairRequestId { get; init; }
    public string RequestStatus { get; init; } = null!;
}