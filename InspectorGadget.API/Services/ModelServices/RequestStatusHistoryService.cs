using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RequestStatusHistoryService : IEntityService<RequestStatusHistory, RequestStatusHistoryDto>
{
    public IDbRepository Repository { get; } = new RequestStatusHistoryRepository();
}