namespace HRManagement.Domain.Entities;

public class UserRole
{
    public Guid UserId { get; set; }

    public string RoleId { get; set; }

    public User UserNavigation { get; set; }

    public Role RoleNavigation { get; set; }
}