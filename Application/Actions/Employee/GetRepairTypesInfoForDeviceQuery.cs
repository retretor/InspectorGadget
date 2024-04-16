using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Composite;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Employee;

public class GetMasterRankingQuery : IRequest<(Result, MasterRankingResult?)>
{
    public DateTime PeriodStart { get; init; }
    public DateTime PeriodEnd { get; init; }
}

public class GetMasterRankingHandler : BaseHandler,
    IRequestHandler<GetMasterRankingQuery, (Result, MasterRankingResult?)>
{
    public GetMasterRankingHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, MasterRankingResult?)> Handle(GetMasterRankingQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await Task.Run(() =>
            DbContext.GetMasterRanking(request.PeriodStart, request.PeriodEnd).SingleOrDefaultAsync());
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(MasterRankingResult),
                (request.PeriodStart, request.PeriodEnd))), null)
            : (Result.Success(), entity);
    }
}

public class GetMasterRankingValidator : AbstractValidator<GetMasterRankingQuery>
{
    public GetMasterRankingValidator()
    {
        RuleFor(v => v.PeriodStart).NotEmpty();
        RuleFor(v => v.PeriodEnd).NotEmpty();
        RuleFor(v => v.PeriodStart).LessThanOrEqualTo(v => v.PeriodEnd);
    }
}