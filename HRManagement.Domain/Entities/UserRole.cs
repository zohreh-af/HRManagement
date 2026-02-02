using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.Domain.Entities;

[Table("tbl_UserRole")]
public class UserRole
{
    [Key]
    [Column("UserId")]
    [StringLength(100)]
    public Guid UserId { get; set; }

    [Key]
    [Column("RoleId")]
    [StringLength(150)]
    public string RoleId { get; set; }
    public User UserNavigation { get; set; }
    public Role RoleNavigation { get; set; }

}