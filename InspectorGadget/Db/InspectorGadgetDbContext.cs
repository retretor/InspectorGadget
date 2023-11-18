using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Db;

public class InspectorGadgetDbContext : DbContext
{
    public InspectorGadgetDbContext(DbContextOptions<InspectorGadgetDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=InspectorGadget;Username=postgres;Password=egoregor");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    // public DbSet<Staff> Staff { get; set; } = null!;
}