using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.Client;

public class DeleteClientCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteClientHandler : BaseHandler, IRequestHandler<DeleteClientCommand, Result>
{
    public DeleteClientHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Clients.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Client), request.Id));
        }

        DbContext.Clients.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteClientValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}