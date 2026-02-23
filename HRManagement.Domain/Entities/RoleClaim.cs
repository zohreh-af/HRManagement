namespace HRManagement.Domain.Entities;

public class RoleClaim
{
    public string Id { get; set; }

    public string RoleId { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    public Role RoleNavigation { get; set; }
}