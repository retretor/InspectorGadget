using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Responses;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairPart;

public class GetRepairPartQuery : IRequest<(Result, Domain.Entities.Basic.RepairPart?)>
{
    public int Id { get; init; }
}

public class GetAllRepairPartsQuery : IRequest<(Result, AllPartsResult?)>
{
}

public class GetRepairPartHandler : BaseHandler,
    IRequestHandler<GetRepairPartQuery, (Result, Domain.Entities.Basic.RepairPart?)>
{
    public GetRepairPartHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.RepairPart?)> Handle(GetRepairPartQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairParts.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(RepairPart), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllRepairPartsHandler : BaseHandler,
    IRequestHandler<GetAllRepairPartsQuery, (Result, AllPartsResult?)>
{
    public GetAllRepairPartsHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, AllPartsResult?)> Handle(GetAllRepairPartsQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await Task.Run(() => DbContext.GetAllParts(), cancellationToken);
        return (Result.Success(), new AllPartsResult { Parts = entities.ToList() });
    }
}

public class GetRepairPartValidator : AbstractValidator<GetRepairPartQuery>
{
    public GetRepairPartValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}