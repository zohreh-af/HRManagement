using HRManagement.Application.Web.Features;
using HRManagement.Infrastructure.Web.Services;
using HRManagement.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorHRManagement.Web.Components.Pages.Account;

public partial class CreateUser
{
    public bool IsSuccessful = false;
    public bool IsInvalid = false;
    public string MassageText = string.Empty;
    public string SuccessMassageText = string.Empty;

    public CreateUserCommand _Request { get; set; } = new CreateUserCommand();
    [Inject] public IHttpClientFactory HttpClientFactory { get; set; }
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
        //var httpClient = HttpClientFactory.CreateClient("HRApi");

        //var response = await httpClient.PostAsJsonAsync("HR/User/CreateUser", _Request);

        //if (response.IsSuccessStatusCode)
        //{
        //    var result = await response.Content.ReadFromJsonAsync<ApiResponse<CreateUserVm>>();
        //    if (result.Data.Result)
        //    {
        //        IsInvalid = false;
        //        IsSuccessful = true;
        //        SuccessMassageText = "User created successfully.";
        //        _Request = new CreateUserCommand(); // Reset the form
        //    }
        //    else
        //    {
        //        IsInvalid = true;
        //        MassageText = "An error occurred while creating the user.";
        //    }
        //}
        //else
        //{
        //    IsInvalid = true;
        //    MassageText = "An error occurred while creating the user.";

        //}
    }
}