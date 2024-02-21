using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairRequest;

public class UpdateRepairRequestCommand : IRequest<Result>
{
    public int Id { get; init; }
    public int DeviceId { get; init; }
    public int ClientId { get; init; }
    public int EmployeeId { get; init; }
    public string SerialNumber { get; init; } = null!;
    public string Description { get; init; } = null!;
    public IApplicationDbContext? DbContext { get; set; }
}

public class UpdateRepairRequestHandler : IRequestHandler<UpdateRepairRequestCommand, Result>
{
    private readonly IMapper _mapper;

    public UpdateRepairRequestHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateRepairRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.RepairRequests.FindAsync(request.Id);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairRequest), request.Id));
        }

        _mapper.Map(request, entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateRepairRequestValidator : AbstractValidator<UpdateRepairRequestCommand>
{
    public UpdateRepairRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Description).NotEmpty();
    }
}