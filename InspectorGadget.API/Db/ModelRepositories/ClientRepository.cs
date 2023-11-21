using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Db.ModelRepositories;

public class ClientRepository : IDbRepository
{
    public ClientRepository()
    {
        Context = new InspectorGadgetContext(new DbContextOptions<InspectorGadgetContext>());
        Context.Database.EnsureCreated();
    }

    public InspectorGadgetContext Context { get; init; }
}