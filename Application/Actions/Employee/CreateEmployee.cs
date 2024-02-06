using MediatR;

namespace Application.Actions.Employee;

public class CreateEmployee : IRequest<int>
{
    public int DbUserId { get; init; }
}