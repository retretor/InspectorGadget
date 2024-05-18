using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.DbUser;

public class DeleteDbUserCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteDbUserHandler : BaseHandler, IRequestHandler<DeleteDbUserCommand, Result>
{
    public DeleteDbUserHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteDbUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.DbUsers.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(DbUser), request.Id));
        }

        DbContext.DbUsers.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteDbUserValidator : AbstractValidator<DeleteDbUserCommand>
{
    public DeleteDbUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}