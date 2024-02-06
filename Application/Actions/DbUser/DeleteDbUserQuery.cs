using Application.Common.Exceptions;
using Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Actions.DbUser;

public class DeleteDbUserQuery : IRequest
{
    public int Id { get; init; }

    public DeleteDbUserQuery(int id) => Id = id;
}

public class DeleteDbUserHandler : IRequestHandler<DeleteDbUserQuery>
{
    private readonly IApplicationDbContext _context;

    public DeleteDbUserHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDbUserQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.DbUsers.FindAsync(request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Basic.DbUser), request.Id);
        }

        _context.DbUsers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class DeleteDbUserValidator : AbstractValidator<DeleteDbUserQuery>
{
    public DeleteDbUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}