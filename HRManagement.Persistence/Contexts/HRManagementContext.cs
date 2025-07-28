using Microsoft.EntityFrameworkCore;
using HRManagement.Domain.Entities;

namespace HRManagement.Persistence.Contexts;

public class HRManagementContext : DbContext
{
    public HRManagementContext(DbContextOptions<HRManagementContext> options)
     : base(options){
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employee { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRManagementContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}