using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Db.ModelRepositories;

public class RepairTypeRepository : IDbRepository
{
    public RepairTypeRepository()
    {
        Context = new InspectorGadgetContext(new DbContextOptions<InspectorGadgetContext>());
        Context.Database.EnsureCreated();
    }

    public InspectorGadgetContext Context { get; init; }
}