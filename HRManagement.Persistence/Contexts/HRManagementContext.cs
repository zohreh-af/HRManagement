using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Persistence.Contexts;

public class HRManagementContext : DbContext
{
    public HRManagementContext(DbContextOptions<HRManagementContext> options)
     : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employee { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRManagementContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.HasOne(u => u.Creator)               
             .WithMany(u => u.CreatedUsers)         
             .HasForeignKey(u => u.CreatorIdentityID)
             .OnDelete(DeleteBehavior.Restrict);   
        });
        modelBuilder.Entity<User>(b =>
        {
            b.HasOne(u => u.LastModifier)            
             .WithMany(u => u.ModifiedUsers)          
             .HasForeignKey(u => u.LastModifierIdentityID)
             .OnDelete(DeleteBehavior.Restrict);     
        });
    }
}