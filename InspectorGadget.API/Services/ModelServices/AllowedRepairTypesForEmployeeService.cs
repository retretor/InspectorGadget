using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.ModelServices;

public class AllowedRepairTypesForEmployeeService : EntityService<AllowedRepairTypesForEmployee, AllowedRepairTypesForEmployeeDto>
{
    public AllowedRepairTypesForEmployeeService(AllowedRepairTypesForEmployeeRepository repository) : base(repository)
    {
    }
}