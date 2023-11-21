using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class ClientService : IEntityService<Client, ClientDto>
{
    public IDbRepository Repository { get; } = new ClientRepository();
}