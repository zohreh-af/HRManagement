using HRManagement.Identity.Client.Base;
using HRManagement.Identity.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorHRManagement.Web.Components.Pages;

public partial class Home : BasePage
{
    [Inject]
    public AuthenticationStateProvider AuthProvider {
        get; set;
    }
    private string username;
    protected override async Task OnInitializedAsync()
    {
        var authProvider = (AppAuthenticationStateProvider)AuthProvider;
        username = await authProvider.GetAuthenticatedUsername();
    }
}