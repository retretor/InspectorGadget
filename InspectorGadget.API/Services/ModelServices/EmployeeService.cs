using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class EmployeeService : EntityService<Employee, CreateEmployeeDto>
{
    private readonly DbUserRepository _userRepository;

    public EmployeeService(EmployeeRepository repository, DbUserRepository dbUserRepository) : base(repository)
    {
        _userRepository = dbUserRepository;
    }

    public new async Task<IEnumerable<EmployeeGetDto>?> Get()
    {
        var employees = await Repository.GetAllAsync();
        if (employees == null) return null;
        var employeesList = employees.ToList();
        if (!employeesList.Any()) return null;
        
        var dbUsers = await _userRepository.GetAllAsync();
        if (dbUsers == null) return null;
        var dbUsersList = dbUsers.ToList();
        if (!dbUsersList.Any()) return null;
        
        var dtos = employeesList.Select(employee =>
        {
            var dbUser = dbUsersList.FirstOrDefault(dbUser => dbUser.Id == employee.DbUserId);
            employee.DbUser = dbUser ?? throw new NullReferenceException();
            return new EmployeeGetDto(employee.Id, employee.DbUser.FirstName,
                employee.DbUser.SecondName, employee.DbUser.TelephoneNumber, employee.DbUser.Login,
                employee.DbUser, employee.RepairRequests, employee.AllowedRepairTypesForEmployees);
        });
        return dtos;
    }
    
    public new async Task<EmployeeGetDto?> Get(int id)
    {
        var employee = await Repository.GetAsync(id);
        if (employee == null) return null;
        
        var dto = new EmployeeGetDto(employee.Id, employee.DbUser.FirstName,
            employee.DbUser.SecondName, employee.DbUser.TelephoneNumber, employee.DbUser.Login,
            employee.DbUser, employee.RepairRequests, employee.AllowedRepairTypesForEmployees);
        return dto;
    }
    
    public new async Task<UpdateEmployeeDto?> Update(int id, UpdateEmployeeDto dto)
    {
        var employee = await Repository.GetAsync(id);
        if (employee == null) return null;
        
        // Update employee table
        employee.AllowedRepairTypesForEmployees = dto.AllowedRepairTypesForEmployees;
        await Repository.UpdateAsync(id, employee);
        
        // Update dbuser table
        employee.DbUser.FirstName = dto.FirstName;
        employee.DbUser.SecondName = dto.SecondName;
        employee.DbUser.TelephoneNumber = dto.TelephoneNumber;
        await _userRepository.UpdateAsync(employee.DbUserId, employee.DbUser);
        return dto;
    }
}