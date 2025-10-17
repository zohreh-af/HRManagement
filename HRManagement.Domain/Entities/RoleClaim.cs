using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.Domain.Entities;

[Table("tbl_UserRole")]
public class RoleClaim
{
    [Key]
    [Column("Id")]
    [StringLength(100)]
    public string Id { get; set; }

    [Key]
    [Column("RoleId")]
    [StringLength(150)]
    public string RoleId { get; set; }

    [Column("ClaimType")]
    [StringLength(100)]
    public string ClaimType { get; set; }

    [Key]
    [Column("ClaimValue")]
    [StringLength(150)]
    public string ClaimValue { get; set; }

    public Role RoleNavigation { get; set; }
}