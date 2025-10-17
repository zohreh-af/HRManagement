using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.Domain.Entities;

[Table("tbl_UserToken")]
public class UserToken
{
    [Key]
    [Column("UserId")]
    [StringLength(100)]
    public string UserId { get; set; }

    [Key]
    [Column("LoginProvider")]
    [StringLength(150)]
    public string LoginProvider { get; set; }

    [Key]
    [Column("Name")]
    [StringLength(100)]
    public string Name { get; set; }

    [Column("Value")]
    [StringLength(100)]
    public string Value { get; set; }
    public User UserNavigation { get; set; }
}