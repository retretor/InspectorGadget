using MediatR;

namespace Application.Actions.RepairType;

public class DeleteRepairType : IRequest
{
    public int Id { get; init; }
}