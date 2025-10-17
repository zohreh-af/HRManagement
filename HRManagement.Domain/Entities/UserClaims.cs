using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.Domain.Entities;
[Table("tbl_UserClaim")]
public class UserClaim
{
    [Key]
    [Column("Id")]
    public string Id { get; set; }

    [Column("ClaimType")]
    [StringLength(50)]
    public string ClaimType { get; set; }

    [Column("ClaimValue")]
    [StringLength(50)]
    public string ClaimValue { get; set; }

    [Column("UserId")]
    [StringLength(50)]
    public string UserId { get; set; }
    public User UserNavigation { get; set; }

}