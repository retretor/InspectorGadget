using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Db.ModelRepositories;

public class RepairRequestRepository : IDbRepository
{
    public RepairRequestRepository()
    {
        Context = new InspectorGadgetContext(new DbContextOptions<InspectorGadgetContext>());
        Context.Database.EnsureCreated();
    }

    public InspectorGadgetContext Context { get; init; }
}