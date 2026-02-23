using System.ComponentModel.DataAnnotations;

namespace HRManagement.Domain.Entities;

public class Role
{
    [Key]
    [StringLength(100)]
    public string Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    public string ConcurrencyStamp { get; set; }

    public ICollection<RoleClaim> RoleClaim { get; set; } = new List<RoleClaim>();

    public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
}