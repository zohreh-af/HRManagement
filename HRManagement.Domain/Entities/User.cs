using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
    public bool EmailConfirmed { get; set; } // بهتر از nullable

    [Column("CreateDate")]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    [Column("PhoneNumber")]
    [StringLength(32)]
    public string? PhoneNumber { get; set; } // به‌جای int

    [Column("PhoneNumberConfirmed")]
    public bool PhoneNumberConfirmed { get; set; }

    [Column("IsActive")]
    public bool IsActive { get; set; }

    [Column("CreatorIdentityID")]
    public Guid? CreatorIdentityID { get; set; }      // ← nullable و بدون StringLength
    public User? Creator { get; set; }

    [Column("LastModifierIdentityID")]
    public Guid? LastModifierIdentityID { get; set; } // ← nullable و بدون StringLength
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

    public ICollection<User> CreatedUsers { get; set; } = new List<User>();
    public ICollection<User> ModifiedUsers { get; set; } = new List<User>();
}
