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
    public int EntityId { get; init; }
    public string Login { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
    public string SecretKey { get; set; } = null!;
    public string Role { get; init; } = null!;
}

public class UpdateDbUserHandler : BaseHandler, IRequestHandler<UpdateDbUserCommand, Result>
{
    public UpdateDbUserHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Result> Handle(UpdateDbUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.DbUsers.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(DbUser), request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateDbUserValidator : AbstractValidator<UpdateDbUserCommand>
{
    public UpdateDbUserValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Login).NotEmpty().MaximumLength(255);
        RuleFor(x => x.PasswordHash).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SecretKey).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Role).NotEmpty().IsEnumName(typeof(Role));
    }
}