using MediatR;

namespace Application.Actions.RepairType;

public class UpdateRepairType : IRequest
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
}