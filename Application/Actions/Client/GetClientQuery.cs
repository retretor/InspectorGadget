using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Client;

public class GetClientQuery : IRequest<Domain.Entities.Basic.Client>
{
    public int Id { get; init; }
}

public class GetClientAllQuery : IRequest<IEnumerable<Domain.Entities.Basic.Client>>
{
}

public class GetClientHandler : IRequestHandler<GetClientQuery, Domain.Entities.Basic.Client>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetClientHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.Client> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.Client>(entity);
    }
}

public class GetAllClientHandler : IRequestHandler<GetClientAllQuery, IEnumerable<Domain.Entities.Basic.Client>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllClientHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Domain.Entities.Basic.Client>> Handle(GetClientAllQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _context.Clients.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<Domain.Entities.Basic.Client>>(entities);
    }
}

public class GetClientValidator : AbstractValidator<GetClientQuery>
{
    public GetClientValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}