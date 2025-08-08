using HRManagement.Application.Web.Features;

namespace HRManagement.Identity.Client.Services;

public class AuthenticationService
{
    public async Task<LoginResult> Authenticate(LoginQuery query)
    {
        var httpClient = HttpClientFactory.CreateClient("HRApi");

        var authenticationResponse = await httpClient.PostAsJsonAsync("HR/User/CreateUser", query);

        if (authenticationResponse.Value.Result == LoginResult.Success)
        {
            await AuthenticationStateProvider.SetUserAuthenticated(authenticationResponse.Value.JwtToken);
        }

        return authenticationResponse.Value.Result;

    }
}