using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class DbUserService : EntityService<DbUser, DbUserAuthDto>
{
    private readonly ClientRepository _clientRepository;
    private readonly EmployeeRepository _employeeRepository;

    public DbUserService(DbUserRepository repository, ClientRepository clientRepository,
        EmployeeRepository employeeRepository) : base(repository)
    {
        _clientRepository = clientRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<DbUserAuthDto?> AuthenticateUser(string login, string password)
    {
        var user = await GetUserByLogin(login);
        if (user == null || password != user.PasswordHash) return null;
        var dto = new DbUserAuthDto(user.Id, user.FirstName, user.SecondName, user.TelephoneNumber, user.Login,
            user.PasswordHash,
            user.Role);
        return dto;
    }

    public async Task<DbUserAuthDto?> RegisterUser(string login, string password, string role, string name,
        string secondName, string telephone)
    {
        var user = await GetUserByLogin(login);
        if (user != null) return null;

        var validRoles = Enum.GetNames(typeof(UserRole));
        if (!validRoles.Contains(role.ToUpper())) return null;
        var newUser = new DbUserAuthDto(0, name, secondName, telephone, login, password, role);
        var createdUser = await Repository.CreateAsync(newUser);
        if (createdUser == null) return null;

        var userRole = Enum.Parse<UserRole>(role.ToUpper());
        switch (userRole)
        {
            case UserRole.CLIENT:
                var client = new CreateClientDto(0, createdUser.Id);
                var createdClient = await _clientRepository.CreateAsync(client);
                if (createdClient == null) return null;
                break;
            case UserRole.ADMIN or UserRole.RECEPTIONIST or UserRole.MASTER:
                var employee = new CreateEmployeeDto(createdUser.Id);
                var createdEmployee = await _employeeRepository.CreateAsync(employee);
                if (createdEmployee == null) return null;
                break;
        }

        var dto = new DbUserAuthDto(createdUser.Id, createdUser.FirstName, createdUser.SecondName,
            createdUser.TelephoneNumber, createdUser.Login,
            createdUser.PasswordHash, createdUser.Role);
        return dto;
    }

    public async Task<DbUserAuthDto?> ChangePassword(string login, string oldPassword, string newPassword)
    {
        var user = await GetUserByLogin(login);
        if (user == null || oldPassword != user.PasswordHash) return null;

        user.PasswordHash = newPassword;
        var dto = new DbUserAuthDto(user.Id, user.FirstName, user.SecondName, user.TelephoneNumber, user.Login,
            user.PasswordHash, user.Role);
        var updatedUser = await Repository.UpdateAsync(dto.Id, dto);
        if (updatedUser == null) return null;
        dto = new DbUserAuthDto(updatedUser.Id, updatedUser.FirstName, updatedUser.SecondName,
            updatedUser.TelephoneNumber, updatedUser.Login, updatedUser.PasswordHash, updatedUser.Role);
        return dto;
    }

    public async Task<DbUserAuthDto?> ChangeInfo(string login, string password, string name,
        string secondName, string telephone, string role)
    {
        var user = await GetUserByLogin(login);
        if (user == null || password != user.PasswordHash) return null;

        user.FirstName = name;
        user.SecondName = secondName;
        user.TelephoneNumber = telephone;
        user.Role = role;
        var dto = new DbUserAuthDto(user.Id, user.FirstName, user.SecondName, user.TelephoneNumber, user.Login,
            user.PasswordHash, user.Role);
        var updatedUser = await Repository.UpdateAsync(dto.Id, dto);
        if (updatedUser == null) return null;
        
        var userRole = Enum.Parse<UserRole>(role.ToUpper());
        switch (userRole)
        {
            case UserRole.CLIENT:
                var client = await _clientRepository.GetAsync(updatedUser.Id);
                if (client == null) return null;
                client.DbUser.FirstName = name;
                client.DbUser.SecondName = secondName;
                client.DbUser.TelephoneNumber = telephone;
                var updatedClient = await _clientRepository.UpdateAsync(client.Id, client);
                if (updatedClient == null) return null;
                break;
            case UserRole.ADMIN or UserRole.RECEPTIONIST or UserRole.MASTER:
                var employee = await _employeeRepository.GetAsync(updatedUser.Id);
                if (employee == null) return null;
                employee.DbUser.FirstName = name;
                employee.DbUser.SecondName = secondName;
                employee.DbUser.TelephoneNumber = telephone;
                var updatedEmployee = await _employeeRepository.UpdateAsync(employee.Id, employee);
                if (updatedEmployee == null) return null;
                break;
        }
        dto = new DbUserAuthDto(updatedUser.Id, updatedUser.FirstName, updatedUser.SecondName,
            updatedUser.TelephoneNumber, updatedUser.Login, updatedUser.PasswordHash, updatedUser.Role);
        return dto;
    }

    public async Task<DbUser?> GetUserByLogin(string login)
    {
        var users = await Repository.GetAllAsync();
        var user = users?.FirstOrDefault(c => c.Login == login);
        return user;
    }
}