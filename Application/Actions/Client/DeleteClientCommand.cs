using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.Client;

public class DeleteClientCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeleteClientHandler : IRequestHandler<DeleteClientCommand, Result>
{
    public DeleteClientHandler()
    {
    }

    public async Task<Result> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }
        var entity = await request.DbContext.Clients.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Client), request.Id));
        }

        request.DbContext.Clients.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        
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