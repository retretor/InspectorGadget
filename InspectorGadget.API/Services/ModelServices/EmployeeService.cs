using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class EmployeeService : IEntityService<Employee, EmployeeDto>
{
    public IDbRepository Repository { get; } = new EmployeeRepository();
}