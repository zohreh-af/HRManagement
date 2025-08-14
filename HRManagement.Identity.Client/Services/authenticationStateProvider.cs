using Blazored.LocalStorage;
using HRManagement.Infrastructure.Web.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HRManagement.Identity.Client.Services;

public class AppAuthenticationStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await localStorage.GetItemAsync<string>(SessionStorageKeys.AuthToken);
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            else
            {
                string? signTime = (await localStorage.GetItemAsync<string>(SessionStorageKeys.SessionStart));

                if ((DateTime.Now - DateTime.Parse(signTime)).TotalHours > 10)
                {
                    return new(new(new ClaimsIdentity()));
                }
            }
            return new(new(new ClaimsIdentity(GetClaims(token), SessionStorageKeys.AuthToken)));
        }
        catch (Exception ex)
        {
            return new(new(new ClaimsIdentity()));
        }
    }

    public async Task SetUserAuthenticated(string token)
    {
        var claims = GetClaims(token); // after loginig in the user vm return jwt token that will pass to the SetUserAuthenticated method . get claim will decode the token and read its data 

        string signTime = DateTime.Now.ToString();

        string userId = claims.FirstOrDefault(p => p.Type == "nameid").Value;

        string username = claims.FirstOrDefault(p => p.Type == "unique_name").Value;


        await localStorage.SetItemAsync(SessionStorageKeys.SecureToken, userId);

        await localStorage.SetItemAsync(SessionStorageKeys.AuthToken, token);

        await localStorage.SetItemAsync(SessionStorageKeys.UserAlias, username);

        await localStorage.SetItemAsync(SessionStorageKeys.SessionStart, signTime);


        //  await ClaimManager.ClearDataLists();

        var authUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        var authState = Task.FromResult(new AuthenticationState(authUser));
        // we need to sent this token to  NotifyAuthenticationStateChanged in a async way but now we already have this value 
        //(Task<AuthenticationState> task); ==> so we need to transform it to a async value and then give it to the method.

        NotifyAuthenticationStateChanged(authState);

    }

    public async Task SetUserLoggedOut()
    {
        await localStorage.RemoveItemAsync(SessionStorageKeys.AuthToken);

        await localStorage.RemoveItemAsync(SessionStorageKeys.UserAlias);

        await localStorage.RemoveItemAsync(SessionStorageKeys.AuthToken);

        await localStorage.RemoveItemAsync(SessionStorageKeys.SessionStart);


        var anonUser = new ClaimsPrincipal(new ClaimsIdentity());
        //An empty ClaimsIdentity means IsAuthenticated == false.

        // Wrapping it in ClaimsPrincipal gives you a “no logged-in user” object.

        var authState = Task.FromResult(new AuthenticationState(anonUser));

        NotifyAuthenticationStateChanged(authState);
        // Task.FromResult wraps the AuthenticationState in a completed task(since this isn’t async work).

        //NotifyAuthenticationStateChanged tells Blazor:

        //“The authentication state has changed — update the UI.”

        //All<AuthorizeView> components and[Authorize] checks will now reflect a logged-out state.
    }

    private IEnumerable<Claim> GetClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();// JwtSecurityTokenHandler is a method that contain a method for reading the body of jwt token (ReadToken)

        var jwtToken = (JwtSecurityToken)handler.ReadToken(token);

        Claim[] claims = jwtToken.Claims.ToArray();

        return claims;
    }
}