using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypesList;

public class CreateRepairTypesListCommand : IRequest<(Result, int?)>
{
    public int RepairTypeForDeviceId { get; init; }
    public int RepairRequestId { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class CreateRepairTypesListHandler : IRequestHandler<CreateRepairTypesListCommand, (Result, int?)>
{
    private readonly IMapper _mapper;

    public CreateRepairTypesListHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<(Result, int?)> Handle(CreateRepairTypesListCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }
        var entity = _mapper.Map<Domain.Entities.Basic.RepairTypesList>(request);
        request.DbContext.RepairTypesLists.Add(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.Id);
    }
}

public class CreateRepairTypesListValidator : AbstractValidator<CreateRepairTypesListCommand>
{
    public CreateRepairTypesListValidator()
    {
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
    }
}