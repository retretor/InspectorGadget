using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.PartForRepairType;

public class CreatePartForRepairTypeCommand : IRequest<(Result, int?)>
{
    public int PartCount { get; init; }
    public int RepairTypeForDeviceId { get; init; }
    public int RepairPartId { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class CreatePartForRepairTypeHandler : IRequestHandler<CreatePartForRepairTypeCommand, (Result, int?)>
{
    private readonly IMapper _mapper;

    public CreatePartForRepairTypeHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<(Result, int?)> Handle(CreatePartForRepairTypeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Basic.PartForRepairType>(request);
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        request.DbContext.PartForRepairTypes.Add(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.Id);
    }
}

public class CreatePartForRepairTypeValidator : AbstractValidator<CreatePartForRepairTypeCommand>
{
    public CreatePartForRepairTypeValidator()
    {
        RuleFor(x => x.PartCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairPartId).NotEmpty();
    }
}