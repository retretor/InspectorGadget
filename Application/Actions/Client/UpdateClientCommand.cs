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
    public IApplicationDbContext? DbContext { get; set; }
}

public class UpdateClientHandler : IRequestHandler<UpdateClientCommand, Result>
{
    private readonly IMapper _mapper;

    public UpdateClientHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.Clients.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Client), request.EntityId));
        }

        _mapper.Map(request, entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

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