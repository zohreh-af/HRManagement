using BlazorHRManagement.Application;

namespace HRManagement.Application.Web.Features;

public class LoginQuery :IQuery<LoginVm>
{
    public string UserName { get; set; }    
    public string Password { get; set; }    
}