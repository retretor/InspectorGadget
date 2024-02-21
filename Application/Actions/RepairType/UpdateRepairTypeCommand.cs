using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairType;

public class UpdateRepairTypeCommand : IRequest<Result>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public IApplicationDbContext? DbContext { get; set; }
}

public class UpdateRepairTypeHandler : IRequestHandler<UpdateRepairTypeCommand, Result>
{
    private readonly IMapper _mapper;

    public UpdateRepairTypeHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateRepairTypeCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.RepairTypes.FindAsync(request.Id);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairType), request.Id));
        }

        _mapper.Map(request, entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}

public class UpdateRepairTypeValidator : AbstractValidator<UpdateRepairTypeCommand>
{
    public UpdateRepairTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}