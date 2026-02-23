namespace HRManagement.Domain.Entities;

public  class UserActionLog
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string Action { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Form { get; set; }

    public string ShamsiDateTime { get; set; }

    public string ShamsiDate { get; set; }

    public User UserNavigation {  get; set;  }
}