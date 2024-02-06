using MediatR;

namespace Application.Actions.RepairTypesList;

public class DeleteRepairTypesList : IRequest
{
    public int Id { get; init; }
}