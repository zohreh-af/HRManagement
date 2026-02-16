using HRManagement.Application.Web;
using HRManagement.Identity.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace HRManagement.Identity.Client.Base;

public class BasePage : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public AppAuthenticationStateProvider AuthState { get; set; }
    [Inject] public IClaimManager ClaimManager { get; set; }

    public async Task CheckAccess()
    {
        if (await IsLoginTimeout(await AuthState.GetAuthenticationStateAsync()))
        {
            NavigationManager.NavigateTo("/Account/Login", true);

            return;
        }

        //if (await ClaimManager.IsUserAdmin())
        //{
        //    return;
        //}
        //to be complete for roles

    }
    private async Task<bool> IsLoginTimeout(AuthenticationState state)
    {
        if (!state.User.Claims.Any())
        {
            return true;
        }

        return false;
    }
}
