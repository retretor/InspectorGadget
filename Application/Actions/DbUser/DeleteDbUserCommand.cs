using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.DbUser;

public class DeleteDbUserCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeleteDbUserHandler : IRequestHandler<DeleteDbUserCommand, Result>
{
    public DeleteDbUserHandler()
    {
    }

    public async Task<Result> Handle(DeleteDbUserCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.DbUsers.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.DbUser), request.Id));
        }

        request.DbContext.DbUsers.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        
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