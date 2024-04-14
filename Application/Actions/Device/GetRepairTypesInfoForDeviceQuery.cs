using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Composite;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Device;

public class GetRepairTypesInfoForDeviceQuery : IRequest<(Result, RepairTypeInfoResult?)>
{
    public int EntityId { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class
    GetRepairTypesInfoForDeviceHandler : IRequestHandler<GetRepairTypesInfoForDeviceQuery, (Result, RepairTypeInfoResult
    ?)>
{
    public async Task<(Result, RepairTypeInfoResult?)> Handle(GetRepairTypesInfoForDeviceQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await Task.Run(() =>
            request.DbContext.GetRepairTypesInfo(request.EntityId).SingleOrDefaultAsync());
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(RepairRequest), request.EntityId)), null)
            : (Result.Success(), entity);
    }
}

public class GetRepairTypesInfoForDeviceValidator : AbstractValidator<GetRepairTypesInfoForDeviceQuery>
{
    public GetRepairTypesInfoForDeviceValidator()
    {
        // TODO: check all validators of EntityId
        RuleFor(v => v.EntityId).NotEmpty();
    }
}