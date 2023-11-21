using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Db.ModelRepositories;

public class EmployeeRepository : IDbRepository
{
    public EmployeeRepository()
    {
        Context = new InspectorGadgetContext(new DbContextOptions<InspectorGadgetContext>());
        Context.Database.EnsureCreated();
    }

    public InspectorGadgetContext Context { get; init; }
}