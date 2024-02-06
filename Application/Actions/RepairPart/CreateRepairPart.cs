using Domain.Enums;
using MediatR;

namespace Application.Actions.RepairPart;

public class CreateRepairPart : IRequest<int>
{
    public string Name { get; init; } = null!;
    public string Specification { get; init; } = null!;
    public int CurrentCount { get; init; }
    public int MinAllowedCount { get; init; }
    public float Cost { get; init; }
    public RepairPartCondition Condition { get; init; }
}