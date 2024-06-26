﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypesList;

public class UpdateRepairTypesListCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public int RepairTypeForDeviceId { get; init; }
    public int RepairRequestId { get; init; }
}

public class UpdateRepairTypesListHandler : BaseHandler, IRequestHandler<UpdateRepairTypesListCommand, Result>
{
    public UpdateRepairTypesListHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Result> Handle(UpdateRepairTypesListCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypesLists.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(
                new NotFoundException(nameof(RepairTypesList), request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateRepairTypesListValidator : AbstractValidator<UpdateRepairTypesListCommand>
{
    public UpdateRepairTypesListValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
    }
}