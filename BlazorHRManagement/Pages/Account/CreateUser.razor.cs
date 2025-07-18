using HRManagement.Application.Web.Features;

namespace BlazorHRManagement.Pages.Account;

public partial class CreateUser
{
    public bool IsInvalid = false;
    public string MassageTitle = string.Empty;
    public CreateUserCommand _Request { get; set; } = new CreateUserCommand();   
    
    public async Task OnValidSubmit(EditContext context)
    {
        AddNewUser();
    }

    public async Task OnInvalidSubmit(EditContext context)
    {
        IsInvalid = true;
        foreach (var message in context.GetValidationMessages())
        {
            MassageTitle = message;
        }
    }

    public async Task AddNewUser()
    {

    }
}
