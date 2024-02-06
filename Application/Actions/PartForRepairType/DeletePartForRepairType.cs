using MediatR;

namespace Application.Actions.PartForRepairType;

public class DeletePartForRepairType : IRequest
{
    public int Id { get; init; }
}