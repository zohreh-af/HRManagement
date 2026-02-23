using HRManagement.Identity.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorHRManagement.Web.Components.Pages;

public partial class Home 
{
    private string username;
    private bool _checked;

    [Inject] public AuthenticationStateProvider AuthProvider {  get; set;
    }

    protected override async Task OnInitializedAsync()
    {
        // await CheckAccess();

       
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var authProvider = (AppAuthenticationStateProvider)AuthProvider;
            username = await authProvider.GetAuthenticatedUsername();

            await CheckAccess();

            _checked = true;

            StateHasChanged();
        }
    }
}