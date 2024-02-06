using MediatR;

namespace Application.Actions.Employee;

public class UpdateEmployee : IRequest<int>
{
    public int Id { get; init; }
    public int DbUserId { get; init; }
}