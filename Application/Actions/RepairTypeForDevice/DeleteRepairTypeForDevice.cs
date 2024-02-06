using MediatR;

namespace Application.Actions.RepairTypeForDevice;

public class DeleteRepairTypeForDevice : IRequest
{
    public int Id { get; init; }
}