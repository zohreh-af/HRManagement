using HRManagement.Application.Web.Features;
using HRManagement.Infrastructure.Web.Services;
using HRManagement.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorHRManagement.Web.Components.Pages.Account;

public partial class CreateUser
{
    public ResponseResult Response { get; set; } = new();
    public bool IsInvalid = false;
    public bool IsLoading = false;
    public string MassageText = string.Empty;

    public CreateUserCommand _Request { get; set; } = new();
    [Inject] public ApiHandler Api { get; set; }

    public async Task OnValidSubmit(EditContext context)
    {
        await AddNewUser();
    }

    public async Task OnInvalidSubmit(EditContext context)
    {
        IsInvalid = true;

        foreach (var message in context.GetValidationMessages())
        {
            MassageText = message;
        }
    }

    public async Task AddNewUser()
    {
        IsLoading = true;

        var result = await Api.SendAsyncObjectByUri<CreateUserVm>(
            HttpMethod.Post,
            "Account/CreateUser",
            _Request);
        if (result.Data.Result)
        {
            Response.Message = TextResources.App_StringKeys_Success_Message;
            Response.Result = true;
        }
        else
        {
            Response.Message = TextResources.App_StringKeys_ٍFailed_Message;
            Response.Result = false;
        }

        IsLoading = false;

    }
}