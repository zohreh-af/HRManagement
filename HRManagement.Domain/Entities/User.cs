using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HRManagement.Domain.Entities;

[Table("tbl_User")]
public class User
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }

    [Column("UserName")]
    [StringLength(50)]
    public string Username { get; set; } = default!;

    [Column("Email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("EmailConfirmed")]
    public bool EmailConfirmed { get; set; }

    [Column("CreateDate")]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    [Column("PhoneNumber")]
    [StringLength(32)]
    public string? PhoneNumber { get; set; } 

    [Column("PhoneNumberConfirmed")]
    public bool PhoneNumberConfirmed { get; set; }

    [Column("IsActive")]
    public bool IsActive { get; set; }

    [Column("CreatorIdentityID")]
    public Guid? CreatorIdentityID { get; set; }    
    public User? Creator { get; set; }

    [Column("LastModifierIdentityID")]
    public Guid? LastModifierIdentityID { get; set; } 
    public User? LastModifier { get; set; }

    [Column("Name")]
    [StringLength(128)]
    public string PersianName { get; set; } = default!;

    [Column("PasswordHash")]
    public string PasswordHash { get; set; } = default!;

    [Column("SecurityStamp")]
    public string? SecurityStamp { get; set; }

    [Column("Details")]
    public string? Details { get; set; }

    public ICollection<UserClaim> UserClaim { get; set; } = new List<UserClaim>();
    public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
    public ICollection<User> CreatedUsers { get; set; } = new List<User>();
    public ICollection<UserLogin> UserLogin { get; set; } = new List<UserLogin>();
    public ICollection<UserToken> UserToken { get; set; } = new List<UserToken>();
    public ICollection<User> ModifiedUsers { get; set; } = new List<User>();
}
