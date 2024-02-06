using MediatR;

namespace Application.Actions.Device;

public class DeleteDevice : IRequest
{
    public int Id { get; init; }
}