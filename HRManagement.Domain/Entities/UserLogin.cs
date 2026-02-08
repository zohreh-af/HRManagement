namespace HRManagement.Domain.Entities;

public class UserLogin
{
    public string LoginProvider { get; set; }

    public string ProviderKey { get; set; }

    public string ProviderDisplayName { get; set; }

    public Guid UserId { get; set; }

    public User UserNavigation { get; set; }
}