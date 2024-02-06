using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class RequestStatusHistoryService : EntityService<RequestStatusHistory, RequestStatusHistoryDto>
{
    public RequestStatusHistoryService(RequestStatusHistoryRepository repository) : base(repository)
    {
    }
}