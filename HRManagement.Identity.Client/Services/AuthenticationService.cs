using HRManagement.Application.Web.Contracts.Identity;
using HRManagement.Application.Web.Features;
using HRManagement.Infrastructure.Web.Services;
using System.Net.Http;
using System.Net.Http.Json;

namespace HRManagement.Identity.Client.Services;

public class AuthenticationService(ApiHandler api, AppAuthenticationStateProvider appAuthenticationStateProvider) :IAuthenticationService
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
}