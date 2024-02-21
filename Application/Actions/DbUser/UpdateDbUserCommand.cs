using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.DbUser;

public class UpdateDbUserCommand : IRequest<Result>
{
    public int Id { get; init; }
    public string Login { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
    public string SecretKey { get; set; } = null!;
    public string Role { get; init; } = null!;
    public IApplicationDbContext? DbContext { get; set; }
}

public class UpdateDbUserHandler : IRequestHandler<UpdateDbUserCommand, Result>
{
    private readonly IMapper _mapper;

    public UpdateDbUserHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateDbUserCommand request, CancellationToken cancellationToken)
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

        _mapper.Map(request, entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateDbUserValidator : AbstractValidator<UpdateDbUserCommand>
{
    public UpdateDbUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Login).NotEmpty().MaximumLength(255);
        RuleFor(x => x.PasswordHash).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SecretKey).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Role).NotEmpty().IsEnumName(typeof(Role));
    }
}