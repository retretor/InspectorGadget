using MediatR;

namespace Application.Actions.RepairRequest;

public class DeleteRepairRequest : IRequest
{
    public int Id { get; init; }
}