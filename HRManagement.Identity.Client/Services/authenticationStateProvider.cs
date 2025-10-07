using Blazored.LocalStorage;
using HRManagement.Infrastructure.Web.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HRManagement.Identity.Client.Services;

public class AppAuthenticationStateProvider(
            ILocalStorageService localStorage,
            AppAuthenticationStateProvider authState,
            IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
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
        var claims = GetClaims(token);

        string signTime = DateTime.Now.ToString();

        string userId = claims.FirstOrDefault(p => p.Type == "nameid").Value;

        string username = claims.FirstOrDefault(p => p.Type == "unique_name").Value;

      
        await localStorage.SetItemAsync(SessionStorageKeys.SecureToken, userId);

        await localStorage.SetItemAsync(SessionStorageKeys.AuthToken, token);

        await localStorage.SetItemAsync(SessionStorageKeys.UserAlias, username);

        await localStorage.SetItemAsync(SessionStorageKeys.SessionStart, signTime);

        var authUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        var authState = Task.FromResult(new AuthenticationState(authUser));
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContextAccessor.HttpContext!.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal,
        new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
        });

        NotifyAuthenticationStateChanged(authState);
    }

    public async Task SetUserLoggedOut()
    {
        await localStorage.RemoveItemAsync(SessionStorageKeys.AuthToken);

        await localStorage.RemoveItemAsync(SessionStorageKeys.UserAlias);

        await localStorage.RemoveItemAsync(SessionStorageKeys.AuthToken);

        await localStorage.RemoveItemAsync(SessionStorageKeys.SessionStart);

        await authState.SetUserLoggedOut();
        await httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var anonUser = new ClaimsPrincipal(new ClaimsIdentity());

        var authenticationState = Task.FromResult(new AuthenticationState(anonUser));

        NotifyAuthenticationStateChanged(authenticationState);
    }

    private IEnumerable<Claim> GetClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();// JwtSecurityTokenHandler is a method that contain a method for reading the body of jwt token (ReadToken)

        var jwtToken = (JwtSecurityToken)handler.ReadToken(token);

        Claim[] claims = jwtToken.Claims.ToArray();

        return claims;
    }
}