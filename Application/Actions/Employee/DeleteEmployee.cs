using MediatR;

namespace Application.Actions.Employee;

public class DeleteEmployee : IRequest
{
    public int Id { get; init; }
}