using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.Client;

public class CreateClientQuery : IRequest<int>
{
    public int DiscountPercentage { get; init; }
    public int DbUserId { get; init; }
}

public class CreateClientHandler : IRequestHandler<CreateClientQuery, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateClientHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateClientQuery request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Basic.Client>(request);
        _context.Clients.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public class CreateClientValidator : AbstractValidator<CreateClientQuery>
{
    public CreateClientValidator()
    {
        RuleFor(x => x.DiscountPercentage).NotEmpty().InclusiveBetween(0, 100);
        RuleFor(x => x.DbUserId).NotEmpty();
    }
}