using HRManagement.Identity.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.RegularExpressions;

namespace HRManagement.Identity.Client.Base;

public class BasePage : ComponentBase
{
    [Inject] public AppAuthenticationStateProvider AuthState { get; set; }
    [Inject] public NavigationManager navigationManager { get; set; }
    public async Task CheckAccess()
    {
        if (await IsLoginTimeout(await AuthState.GetAuthenticationStateAsync()))
        {
            navigationManager.NavigateTo("/Account/Login", true);

            return;
        }

        IsUserHaveAccess();
    }

    private async Task<bool> IsLoginTimeout(AuthenticationState state)
    {
        if (!state.User.Claims.Any())
        {
            return true;
        }

        return false;
    }
    public async Task<bool> IsUserHaveAccess () 
    {
        string currentUrl = navigationManager.Uri;
        var relativePath = navigationManager.ToBaseRelativePath(currentUrl);

        var allowedLinks = new List<string>
        {
            "/account/login",
            "/account/createuser"
        };

        if (allowedLinks.Contains(relativePath, StringComparer.OrdinalIgnoreCase))
        {
            return true; 
        }
        navigationManager.NavigateTo("/account/login", true);
        return false;
    }
}