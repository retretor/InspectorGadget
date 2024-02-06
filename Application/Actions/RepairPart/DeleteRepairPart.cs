using MediatR;

namespace Application.Actions.RepairPart;

public class DeleteRepairPart : IRequest
{
    public int Id { get; init; }
}