using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }

    public AppDbContext()
    {
    }

    public virtual DbSet<OneOnOne> OneOnOnes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
