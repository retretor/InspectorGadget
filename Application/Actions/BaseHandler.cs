using Application.Common.Interfaces;
using AutoMapper;

namespace Application.Actions;

public abstract class BaseHandler
{
    protected readonly IApplicationDbContext DbContext;
    protected readonly IMapper? Mapper;

    protected BaseHandler(IApplicationDbContext dbContext, IMapper? mapper = null)
    {
        DbContext = dbContext;
        Mapper = mapper;
    }
}