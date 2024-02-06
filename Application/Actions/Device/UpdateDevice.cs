using Domain.Enums;
using MediatR;

namespace Application.Actions.Device;

public class UpdateDevice : IRequest<int>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public DeviceType Type { get; init; }
    public string Brand { get; init; } = null!;
    public string Series { get; init; } = null!;
    public string Manufacturer { get; init; } = null!;
}