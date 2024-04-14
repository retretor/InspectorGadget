using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Employee;

public class UpdateEmployeeRoleCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public string Role { get; set; } = null!;
}

public class UpdateEmployeeRoleHandler : BaseHandler, IRequestHandler<UpdateEmployeeRoleCommand, Result>
{
    public UpdateEmployeeRoleHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(UpdateEmployeeRoleCommand request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
            DbContext.UpdateEmployeeRole(request.EntityId, request.Role).SingleOrDefaultAsync());

        return Result.Success();
    }
}

public class UpdateEmployeeRoleValidator : AbstractValidator<UpdateEmployeeRoleCommand>
{
    public UpdateEmployeeRoleValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.Role).NotEmpty().IsEnumName(typeof(Role));
    }
}