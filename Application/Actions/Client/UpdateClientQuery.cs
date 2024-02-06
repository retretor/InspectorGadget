using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.Client;

public class UpdateClientQuery : IRequest
{
    public int Id { get; init; }
    public int DiscountPercentage { get; init; }
    public int DbUserId { get; init; }
}

public class UpdateClientHandler : IRequestHandler<UpdateClientQuery>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateClientHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(UpdateClientQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Basic.Client), request.Id);
        }
        
        _mapper.Map(request, entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateClientValidator : AbstractValidator<UpdateClientQuery>
{
    public UpdateClientValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DiscountPercentage).NotEmpty().InclusiveBetween(0, 100);
        RuleFor(x => x.DbUserId).NotEmpty();
    }
}