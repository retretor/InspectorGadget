using Application.Common.Exceptions;
using Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Actions.Client;

public class DeleteClientQuery : IRequest
{
    public int Id { get; init; }
}

public class DeleteClientHandler : IRequestHandler<DeleteClientQuery>
{
    private readonly IApplicationDbContext _context;

    public DeleteClientHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteClientQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients.FindAsync(request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Basic.Client), request.Id);
        }

        _context.Clients.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class DeleteClientValidator : AbstractValidator<DeleteClientQuery>
{
    public DeleteClientValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}