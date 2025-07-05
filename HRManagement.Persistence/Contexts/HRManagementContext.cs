using Microsoft.EntityFrameworkCore;
using HRManagement.Domain.Entities;

namespace HRManagement.Persistence.Contexts;

public class HRManagementContext : DbContext
{
    public HRManagementContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employee { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Employee>()
                    .HasOne(u => u.User)
                    .WithMany() 
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);


        base.OnModelCreating(modelBuilder);
    }
}