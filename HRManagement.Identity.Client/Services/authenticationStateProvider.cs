using Blazored.LocalStorage;
using HRManagement.Application.Api.Features;
using HRManagement.Infrastructure.Web.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HRManagement.Identity.Client.Services;

public class AppAuthenticationStateProvider(
            ILocalStorageService localStorage,
            IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
{
    private List<GetUserClaimsByTokenDto> claims;
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

                if (string.IsNullOrWhiteSpace(signTime) || (DateTime.Now - DateTime.Parse(signTime)).TotalHours > 10)
                {
                    return new(new(new ClaimsIdentity()));
                }
            }
            return new(new(new ClaimsIdentity(GetClaims(token), SessionStorageKeys.AuthToken)));
        }
        catch (Exception ex)
        {
            //logger
            return new(new(new ClaimsIdentity()));
        }
    }

    public async Task SetUserAuthenticated(string token)
    {
        var claims = GetClaims(token);
        await localStorage.SetItemAsync(SessionStorageKeys.AuthToken, token);
        await localStorage.SetItemAsync(SessionStorageKeys.UserAlias, claims.FirstOrDefault(c => c.Type == "unique_name")?.Value);
        await localStorage.SetItemAsync(SessionStorageKeys.SessionStart, DateTime.Now.ToString());

        //var authUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        var authUser = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var authState = Task.FromResult(new AuthenticationState(authUser));
     //   var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //var principal = new ClaimsPrincipal(identity);

        //await GetUserClaims();
        //await httpContextAccessor.HttpContext!.SignInAsync(
        //CookieAuthenticationDefaults.AuthenticationScheme,
        //principal,
        //new AuthenticationProperties
        //{
        //    IsPersistent = true,
        //    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
        //});

        NotifyAuthenticationStateChanged(authState);
    }

    public async Task SetUserLoggedOut()
    {
        await localStorage.RemoveItemAsync(SessionStorageKeys.UserAlias);

        await localStorage.RemoveItemAsync(SessionStorageKeys.AuthToken);

        await localStorage.RemoveItemAsync(SessionStorageKeys.SessionStart);

        await ClearDataLists();

        await httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var anonUser = new ClaimsPrincipal(new ClaimsIdentity());

        var authenticationState = Task.FromResult(new AuthenticationState(anonUser));

        NotifyAuthenticationStateChanged(authenticationState);
    }
    public async Task<string> GetAuthenticatedUsername()
    {
        var username = await localStorage.GetItemAsync<string>(SessionStorageKeys.UserAlias);

        return username;
    }
    private IEnumerable<Claim> GetClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();// JwtSecurityTokenHandler is a method that contain a method for reading the body of jwt token (ReadToken)

        var jwtToken = (JwtSecurityToken)handler.ReadToken(token);

        Claim[] claims = jwtToken.Claims.ToArray();

        return claims;
    }
    public async Task ClearDataLists()
    {
        claims = null;

        //roles = null;

        //isAdmin = null;
    }

}