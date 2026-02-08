namespace HRManagement.Domain.Entities;

public class Role
{
    [Key]
    public string Id { get; set; }

    public string Name { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string NormalizedName { get; set; }

    public ICollection<RoleClaim> RoleClaim { get; set; } = new List<RoleClaim>();

    public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
}