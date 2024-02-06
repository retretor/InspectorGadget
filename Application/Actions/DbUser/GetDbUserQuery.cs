using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.DbUser;

public class GetDbUserQuery : IRequest<Domain.Entities.Basic.DbUser>
{
    public int Id { get; init; }

    public GetDbUserQuery(int id)
    {
        Id = id;
    }
}

public class GetAllDbUserQuery : IRequest<IEnumerable<Domain.Entities.Basic.DbUser>>
{
}

public class GetDbUserHandler : IRequestHandler<GetDbUserQuery, Domain.Entities.Basic.DbUser>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDbUserHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.DbUser> Handle(GetDbUserQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.DbUsers.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.DbUser>(entity);
    }
}

public class GetAllDbUserHandler : IRequestHandler<GetAllDbUserQuery, IEnumerable<Domain.Entities.Basic.DbUser>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllDbUserHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Domain.Entities.Basic.DbUser>> Handle(GetAllDbUserQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _context.DbUsers.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<Domain.Entities.Basic.DbUser>>(entities);
    }
}

public class GetDbUserValidator : AbstractValidator<GetDbUserQuery>
{
    public GetDbUserValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}