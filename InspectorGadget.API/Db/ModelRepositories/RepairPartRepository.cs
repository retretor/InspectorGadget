using InspectorGadget.DTOs;
using InspectorGadget.Models;
using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Db.ModelRepositories;

public class RepairPartRepository : IDbRepository
{
    public RepairPartRepository()
    {
        Context = new InspectorGadgetContext(new DbContextOptions<InspectorGadgetContext>());
        Context.Database.EnsureCreated();
    }

    public InspectorGadgetContext Context { get; init; }
}