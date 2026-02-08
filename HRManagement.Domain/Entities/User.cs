using HRManagement.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = default!;

    public string? Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public string? PhoneNumber { get; set; } 

    public bool PhoneNumberConfirmed { get; set; }

    public bool IsActive { get; set; }

    public Guid? CreatorIdentityID { get; set; }    

    public User? Creator { get; set; }

    public Guid? LastModifierIdentityID { get; set; } 

    public User? LastModifier { get; set; }

    public string PersianName { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public string? SecurityStamp { get; set; }

    public string? Details { get; set; }

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