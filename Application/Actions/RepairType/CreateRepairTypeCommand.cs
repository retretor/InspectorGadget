using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairType;

public class CreateRepairTypeCommand : IRequest<(Result, int?)>
{
    public string Name { get; init; } = null!;
    public IApplicationDbContext? DbContext { get; set; }
}

public class CreateRepairTypeHandler : IRequestHandler<CreateRepairTypeCommand, (Result, int?)>
{
    private readonly IMapper _mapper;

    public CreateRepairTypeHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<(Result, int?)> Handle(CreateRepairTypeCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = _mapper.Map<Domain.Entities.Basic.RepairType>(request);
        request.DbContext.RepairTypes.Add(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.Id);
    }
}

public class CreateRepairTypeValidator : AbstractValidator<CreateRepairTypeCommand>
{
    public CreateRepairTypeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}