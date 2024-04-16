using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.Client;

public class UpdateClientCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public string FirstName { get; init; } = null!;
    public string SecondName { get; init; } = null!;
    public string TelephoneNumber { get; init; } = null!;
    public int DiscountPercentage { get; init; }
}

public class UpdateClientHandler : BaseHandler, IRequestHandler<UpdateClientCommand, Result>
{
    public UpdateClientHandler(IMapper mapper, IApplicationDbContext dbContext) : base(dbContext, mapper)
    {
    }

    public async Task<Result> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Clients.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Client), request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateClientValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SecondName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TelephoneNumber).NotEmpty()
            .Must(x => x.StartsWith("0") && x.Length == 10 && x.All(char.IsDigit));
        RuleFor(x => x.DiscountPercentage).InclusiveBetween(0, 100);
    }
}