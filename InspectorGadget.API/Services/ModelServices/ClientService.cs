using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class ClientService : EntityService<Client, CreateClientDto>
{
    private readonly DbUserRepository _dbUserRepository;

    public ClientService(ClientRepository repository, DbUserRepository dbUserRepository) : base(repository)
    {
        _dbUserRepository = dbUserRepository;
    }

    public new async Task<IEnumerable<ClientGetDto>?> Get()
    {
        var clients = await Repository.GetAllAsync();
        if (clients == null) return null;
        var clientsList = clients.ToList();
        if (!clientsList.Any()) return null;
        
        var dbUsers = await _dbUserRepository.GetAllAsync();
        if (dbUsers == null) return null;
        var dbUsersList = dbUsers.ToList();
        if (!dbUsersList.Any()) return null;
        
        var dtos = clientsList.Select(client =>
        {
            var dbUser = dbUsersList.FirstOrDefault(dbUser => dbUser.Id == client.DbUserId);
            client.DbUser = dbUser ?? throw new NullReferenceException();
            return new ClientGetDto(client.DbUserId, client.DbUser.FirstName,
                client.DbUser.SecondName, client.DbUser.TelephoneNumber, client.DbUser.Login,
                client.DiscountPercentage, client.DbUser, client.RepairRequests);
        });
        return dtos;
    }

    public new async Task<ClientGetDto?> Get(int id)
    {
        var client = await Repository.GetAsync(id);
        if (client == null) return null;

        var dto = new ClientGetDto(client.DbUserId, client.DbUser.FirstName, client.DbUser.SecondName,
            client.DbUser.TelephoneNumber, client.DbUser.Login, client.DiscountPercentage, client.DbUser,
            client.RepairRequests);
        return dto;
    }

    public new async Task<UpdateClientDto?> Update(int id, UpdateClientDto dto)
    {
        var client = await Repository.GetAsync(id);
        if (client == null) return null;

        // Update client table
        client.DiscountPercentage = dto.DiscountPercentage;
        await Repository.UpdateAsync(id, client);
        
        // Update dbuser table
        client.DbUser.FirstName = dto.FirstName;
        client.DbUser.SecondName = dto.SecondName;
        client.DbUser.TelephoneNumber = dto.TelephoneNumber;
        await _dbUserRepository.UpdateAsync(client.DbUserId, client.DbUser);
        return dto;
    }
    
    public new async Task Delete(int id)
    {
        var client = await Repository.GetAsync(id);
        if (client == null) return;
        await Repository.DeleteAsync(id);
        await _dbUserRepository.DeleteAsync(client.DbUserId);
    }
}