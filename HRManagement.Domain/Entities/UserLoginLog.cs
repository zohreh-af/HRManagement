namespace HRManagement.Domain.Entities;

public class UserLoginLog
{
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public string UserName { get; set; }

    public DateTime LoginDateTime { get; set; }  

    public string ShamsiDateTime { get; set; }

    public string ShamsiDate { get; set; }  

    public string DeviceIp { get; set; }

    public User UserNavigation {  get; set; }
}