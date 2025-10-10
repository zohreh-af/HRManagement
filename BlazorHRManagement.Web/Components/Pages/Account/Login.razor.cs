using Azure.Core;
using HRManagement.Application.Web.Contracts.Identity;
using HRManagement.Application.Web.Features;
using HRManagement.Identity.Client.Base;
using HRManagement.Identity.Client.Services;
using HRManagement.Infrastructure.Web.Services;
using HRManagement.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
namespace BlazorHRManagement.Web.Components.Pages.Account;


public partial class Login 
{
    public ResponseResult Response { get; set; } = new();
    public bool IsInvalid = false;
    public bool IsLoading = false;
    public string MassageText = string.Empty;

    public LoginUserQuery _Request { get; set; } = new();

    [Inject] public IAuthenticationService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ApiHandler Api { get; set; }
    protected override async Task OnInitializedAsync()
    {
        //IsCaptchaActive = bool.Parse(Configuration["CaptchaSettings:IsCaptchaActive"]);
#if DEBUG
        _Request.Username = "admin";

        _Request.Password = "rfidadmin";
#endif
    }


    public async Task OnValidSubmit(EditContext context)
    {
        await LoginUser(_Request);
    }

    public async Task OnInvalidSubmit(EditContext context)
    {
        IsInvalid = true;

        foreach (var message in context.GetValidationMessages())
        {
            MassageText = message;
        }
    }

    public async Task LoginUser(LoginUserQuery _Request)
    {
        IsLoading = true;

        var result = await AuthService.Authenticate(_Request);

        IsLoading = false;

        switch (result)
        {
            case LoginResult.Success:
                NavigationManager.NavigateTo("/", true);
                break;

            //case LoginResult.InvalidCredentials:
            //    Notification.AddNotification(TextResources.APP_StringKeys_Message_LoginFailed
            //        , NotificationType.Error);
            //    break;

            //case LoginResult.IpBanned:
            //    Notification.AddNotification(TextResources.APP_StringKeys_Ip_Banned
            //        , NotificationType.Error);
            //    break;

            //case LoginResult.UserLocked:
            //    Notification.AddNotification(TextResources.APP_StringKeys_User_Locked
            //       , NotificationType.Error);
            //    break;
        }
                IsLoading = false;
    }
}