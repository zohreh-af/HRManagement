using HRManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser<Guid>
{
    [StringLength(50)]
    public string PersianName { get; set; } = default!;

    public bool IsActive { get; set; }

    public Guid? CreatorIdentityID { get; set; }
    public User? Creator { get; set; }

    public Guid? LastModifierIdentityID { get; set; }
    public User? LastModifier { get; set; }

    // Navigation collections
    public ICollection<UserClaim> UserClaimUsers { get; set; } = new List<UserClaim>();
    public ICollection<UserRole> UserRoleUsers { get; set; } = new List<UserRole>();
    public ICollection<User> CreatedUsers { get; set; } = new List<User>();
    public ICollection<UserActionLog> UserActionLogUsers { get; set; } = new List<UserActionLog>();
    public ICollection<UserLogin> UserLoginUsers { get; set; } = new List<UserLogin>();
    public ICollection<UserLoginLog> UserLoginLogUsers { get; set; } = new List<UserLoginLog>();
    public ICollection<UserToken> UserTokenUsers { get; set; } = new List<UserToken>();
    public ICollection<User> ModifiedUsers { get; set; } = new List<User>();
    public ICollection<Employee> EmployeeUsers { get; set; } = new List<Employee>();
}