using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.DbUser;

public class UpdateDbUserQuery : IRequest
{
    public int Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string SecondName { get; init; } = null!;
    public string TelephoneNumber { get; init; } = null!;
    public string Login { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
    public Role Role { get; init; }
}

public class UpdateDbUserHandler : IRequestHandler<UpdateDbUserQuery>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateDbUserHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(UpdateDbUserQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.DbUsers.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Basic.DbUser), request.Id);
        }
        
        _mapper.Map(request, entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}


public class UpdateDbUserValidator : AbstractValidator<UpdateDbUserQuery>
{
    public UpdateDbUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SecondName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TelephoneNumber).NotEmpty()
            .Must(x => x.StartsWith("0") && x.Length == 10 && x.All(char.IsDigit));
        RuleFor(x => x.Login).NotEmpty().MaximumLength(255);
        RuleFor(x => x.PasswordHash).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Role).NotEmpty();
    }
}