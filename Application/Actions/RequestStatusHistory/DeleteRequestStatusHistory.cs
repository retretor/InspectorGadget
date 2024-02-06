using MediatR;

namespace Application.Actions.RequestStatusHistory;

public class DeleteRequestStatusHistory : IRequest
{
    public int Id { get; init; }
}