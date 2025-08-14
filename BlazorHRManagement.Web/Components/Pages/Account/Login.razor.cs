using HRManagement.Application.Web.Features;
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

    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ApiHandler Api { get; set; }

    public async Task OnValidSubmit(EditContext context)
    {
        await LoginUser();
    }

    public async Task OnInvalidSubmit(EditContext context)
    {
        IsInvalid = true;

        foreach (var message in context.GetValidationMessages())
        {
            MassageText = message;
        }
    }

    public async Task LoginUser()
    {
        IsLoading = true;

        var result = await Api.SendAsyncObjectByUri<LoginUserVm>(
            HttpMethod.Post,
            "Account/LoginUser",
            _Request);
        if (result.Data.Result == LoginResult.Success)
        {
            NavigationManager.NavigateTo("/");
        }
        else
        {
            Response.Message = TextResources.App_StringKeys_ٍFailed_Message;
            Response.Result = false;
        }

        IsLoading = false;
    }
}