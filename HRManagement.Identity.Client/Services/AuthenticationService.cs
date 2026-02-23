using HRManagement.Application.Web.Features;
using HRManagement.Infrastructure.Web.Services;

namespace HRManagement.Identity.Client.Services;

public class AuthenticationService(ApiHandler api, AppAuthenticationStateProvider appAuthenticationStateProvider) 
    : HRManagement.Application.Web.Contracts.Identity.IAuthenticationService
{
    public async Task<LoginResult> Authenticate(LoginUserQuery query)
    {
        var response = await api.SendAsyncObjectByUri<LoginUserVm>(HttpMethod.Post
            , "Account/LoginUser", query);


        if (response.Data.Result == LoginResult.Success)
        {
            await appAuthenticationStateProvider.SetUserAuthenticated(response.Data.JwtToken);
        }

        return response.Data.Result;

    }
    public async Task Logout()
    {
        await appAuthenticationStateProvider.SetUserLoggedOut();
    }
}