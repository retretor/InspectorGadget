using MediatR;

namespace Application.Actions.RepairType;

public class CreateRepairType : IRequest<int>
{
    public string Name { get; init; } = null!;
}