using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.Domain.Entities;

public class Role
{
    [Key]
    [Column("Id")]
    [StringLength(100)]
    public string Id { get; set; }

    [Column("Name")]
    [StringLength(100)]
    public string Name { get; set; }

    [Column("ConcurrencyStamp")]
    [StringLength(100)]
    public string ConcurrencyStamp { get; set; }

    [Column("NormalizedName")]
    [StringLength(150)]
    public string NormalizedName { get; set; }

    public ICollection<RoleClaim> RoleClaim { get; set; } = new List<RoleClaim>();
    public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
}