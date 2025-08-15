using Abstraction;
using BlazorHRManagement.Infrastructure.Api.Utilities;
using HRManagement.Application.Web.Features;
using HRManagement.Domain.Entities;
using HRManagement.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HRManagement.Application.Api.Features.Account.Queries.LoginUser;

public class LoginUserHandler(HRManagementContext context,IConfiguration configuration
    , IPasswordHasher<User> passwordHasher): IQueryHandler<LoginUserQuery, LoginUserVm>
{
    public async Task<LoginUserVm> HandleAsync(LoginUserQuery query, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync
                            (u => u.Username.ToLower() == query.Username.ToLower(),
                             cancellationToken);

        if (user is null)
        {
            return new()
            {
                JwtToken = null,
                Result = LoginResult.UserNotFound
            };
        }
        var isPasswordValid = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, query.Password);

        if (isPasswordValid != PasswordVerificationResult.Success)
        {
            return new()
            {
                JwtToken = null,
                Result = LoginResult.FailedToLogin
            };
        }

        ClaimsIdentity claimsIdentity = await GetUserClaims(user);

        return new()
        {
            JwtToken = GetJwtToken(claimsIdentity),
            Result = LoginResult.Success
        };

    }

    private string GetJwtToken(ClaimsIdentity? claimsIdentity)
    {
        SecurityTokenDescriptor descriptor = new()
        {
            Issuer = "HRIdentity",
            Audience = "HRTicketIdentityUser",
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow.AddMinutes(0),
            Expires = DateTime.UtcNow.AddHours(10),
            SigningCredentials = CryptoTools.GetJwtCredential(configuration["Jwt:Secret"]),
            Claims = claimsIdentity.Claims.ToDictionary(c => c.Type, c => (object)c.Value)
        };

        JwtSecurityTokenHandler tokenHandler = new();

        SecurityToken securityToken = tokenHandler.CreateToken(descriptor);

        string jwt = tokenHandler.WriteToken(securityToken);

        return jwt;
    }

    private async Task<ClaimsIdentity> GetUserClaims(User user)
    {
        var claims = new List<Claim>();

        claims.Add(new(ClaimTypes.Name, user.Username));
        claims.Add(new(ClaimTypes.NameIdentifier, user.Id.ToString()));
        //user rolle 
        claims.Add(new(ClaimTypes.Surname, user.PersianName));

        return new ClaimsIdentity(claims);
    }
}