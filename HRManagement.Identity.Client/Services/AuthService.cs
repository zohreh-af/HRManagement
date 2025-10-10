using HRManagement.Application.Web.Features;
using HRManagement.Infrastructure.Web.Services;
using HRManagement.Application.Web.Contracts.Identity;

namespace HRManagement.Identity.Client.Services;

public class AuthService(ApiHandler Api
                         , AppAuthenticationStateProvider AuthenticationStateProvider)
                        : IAuthenticationService
{
    public async Task<LoginResult> Authenticate(LoginUserQuery query)
    {
        var authenticationResponse = await Api.SendAsyncObjectByUri<LoginUserVm>(HttpMethod.Post
            , "Account/UserLogin"
            , query);

        if (authenticationResponse.Data.Result == LoginResult.Success)
        {
            await AuthenticationStateProvider.SetUserAuthenticated(authenticationResponse.Data.JwtToken);
        }

        return authenticationResponse.Data.Result;
    }

    public async Task Logout()
    {
        await AuthenticationStateProvider.SetUserLoggedOut();
    }
}