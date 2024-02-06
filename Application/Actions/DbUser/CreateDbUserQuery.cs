using Application.Common.Interfaces;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.DbUser;

public class CreateDbUserQuery : IRequest<int>
{
    public string FirstName { get; init; } = null!;
    public string SecondName { get; init; } = null!;
    public string TelephoneNumber { get; init; } = null!;
    public string Login { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
    public Role Role { get; init; }
}

public class CreateDbUserHandler : IRequestHandler<CreateDbUserQuery, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateDbUserHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateDbUserQuery request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Basic.DbUser>(request);
        _context.DbUsers.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public class CreateDbUserValidator : AbstractValidator<CreateDbUserQuery>
{
    public CreateDbUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SecondName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TelephoneNumber).NotEmpty()
            .Must(x => x.StartsWith("0") && x.Length == 10 && x.All(char.IsDigit));
        RuleFor(x => x.Login).NotEmpty().MaximumLength(255);
        RuleFor(x => x.PasswordHash).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Role).NotEmpty();
    }
}