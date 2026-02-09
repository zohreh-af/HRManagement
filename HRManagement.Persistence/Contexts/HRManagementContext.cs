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
    public DbSet<UserActionLog> UserActionLog { get; set; }
    public DbSet<UserClaim> UserClaim { get; set; }
    public DbSet<UserLogin> UserLogin { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<UserToken> UserToken { get; set; }
    public DbSet<Role> Role { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRManagementContext).Assembly);

        modelBuilder.Entity<User>(b =>
        {
            b.HasOne(u => u.Creator)
             .WithMany(u => u.CreatedUsers)
             .HasForeignKey(u => u.CreatorIdentityID)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(u => u.LastModifier)
             .WithMany(u => u.ModifiedUsers)
             .HasForeignKey(u => u.LastModifierIdentityID)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserToken>()
                    .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        modelBuilder.Entity<UserRole>()
                    .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserLogin>()
                    .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        modelBuilder.Entity<UserClaim>()
            .HasOne(uc => uc.UserNavigation)
            .WithMany(u => u.UserClaimUsers)
            .HasForeignKey(uc => uc.UserId)
            .HasPrincipalKey(u => u.Id);

        modelBuilder.Entity<UserLogin>()
            .HasOne(uc => uc.UserNavigation)
            .WithMany(u => u.UserLoginUsers)
            .HasForeignKey(uc => uc.UserId)
            .HasPrincipalKey(u => u.Id);

        modelBuilder.Entity<UserToken>()
            .HasOne(uc => uc.UserNavigation)
            .WithMany(u => u.UserTokenUsers)
            .HasForeignKey(uc => uc.UserId)
            .HasPrincipalKey(u => u.Id);

        modelBuilder.Entity<UserRole>()
            .HasOne(uc => uc.UserNavigation)
            .WithMany(u => u.UserRoleUsers)
            .HasForeignKey(uc => uc.UserId)
            .HasPrincipalKey(u => u.Id);

        modelBuilder.Entity<UserRole>()
            .HasOne(uc => uc.RoleNavigation)
            .WithMany(u => u.UserRole)
            .HasForeignKey(uc => uc.RoleId)
            .HasPrincipalKey(u => u.Id);

        modelBuilder.Entity<RoleClaim>()
            .HasOne(uc => uc.RoleNavigation)
            .WithMany(u => u.RoleClaim)
            .HasForeignKey(uc => uc.RoleId)
            .HasPrincipalKey(u => u.Id);

        base.OnModelCreating(modelBuilder);


    }
}