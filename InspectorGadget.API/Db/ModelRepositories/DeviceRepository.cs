using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Db.ModelRepositories;

public class DeviceRepository : IDbRepository
{
    public DeviceRepository()
    {
        Context = new InspectorGadgetContext(new DbContextOptions<InspectorGadgetContext>());
        Context.Database.EnsureCreated();
    }

    public InspectorGadgetContext Context { get; init; }
}