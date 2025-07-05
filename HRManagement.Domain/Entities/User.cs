using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.Domain.Entities;

[Table("tbl_User")]
public class User
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }

    [Column("UserName")]
    [StringLength(100)]
    public string? UserName { get; set; }

    [Column("Email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("EmailConfirmed")]
    [DefaultValue(false)]
    public bool EmailConfirmed { get; set; }

    [Column("CreateDate")]
    public DateTime? CreateDate { get; set; }

    [Column("PhoneNumber")]
    public int PhoneNumber { get; set; }

    [Column("PhoneNumberConfirmed")]
    [DefaultValue(false)]
    public bool PhoneNumberConfirmed { get; set; }

    [Column("IsActive")]
    [DefaultValue(false)]
    public bool IsActive { get; set; }

    [Column("CreatorIdentityID")]
    [StringLength(50)]
    public string CreatorIdentityID { get; set; }

    [Column("LastModifierIdentityID")]
    [StringLength(128)]
    public string? LastModifierIdentityID { get; set; }

    [Column("Name")]
    [StringLength(128)]
    public string Name { get; set; }

    [Column("PasswordHash")]
    public string? PasswordHash { get; set; }

    [Column("SecurityStamp")]
    public string? SecurityStamp { get; set; }

    [Column("Details")]
    public string? Details { get; set; }
}