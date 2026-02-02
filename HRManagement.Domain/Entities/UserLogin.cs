using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.Domain.Entities;

[Table("tbl_UserLogin")]
public class UserLogin
{
    [Key]
    [Column("LoginProvider")]
    [StringLength(100)]
    public string LoginProvider { get; set; }

    [Column("ProviderKey")]
    [StringLength(150)]
    public string ProviderKey { get; set; }

    [Column("ProviderDisplayName")]
    [StringLength(100)]
    public string ProviderDisplayName { get; set; }

    [Column("UserId")]
    [StringLength(50)]
    public Guid UserId { get; set; }
    public User UserNavigation { get; set; }
}