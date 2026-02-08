namespace HRManagement.Domain.Entities;
public class UserClaim
{
    public string Id { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    public Guid UserId { get; set; }

    public User UserNavigation { get; set; }
}