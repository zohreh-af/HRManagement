using Abstraction;
using Abstraction.Abstraction;
using HRManagement.Application.Web.Features;
using HRManagement.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace HRManagement.Application.Api.Features.Account.Queries.LoginUser;

public class LoginUserHandler: IQueryHandler<LoginUserQuery, LoginUserVm>
{
    private readonly HRManagementContext _context;
    //private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtGenerator _jwtGenerator;

    public LoginUserHandler(
        HRManagementContext context,
        IConfiguration configuration,
        IPasswordHasher<User> passwordHasher,
        IJwtGenerator jwtGenerator)
    {
        _context = context;
        //_configuration = configuration;
        _passwordHasher = passwordHasher;
        _jwtGenerator = jwtGenerator;
    }
    public async Task<LoginUserVm> HandleAsync(LoginUserQuery query, CancellationToken cancellationToken )
    {
        var user = await _context.Users.FirstOrDefaultAsync
                            (u => u.Username.ToLower() == query.Email.ToLower(),
                             cancellationToken);

        if (user is null)
        {
            return new()
            {
                JwtToken = null,
                Result = LoginResult.UserNotFound
            };
        }
        var isPasswordValid = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, query.Password);

        if (isPasswordValid != PasswordVerificationResult.Success)
        {
            return new()
            {
                JwtToken = null,
                Result = LoginResult.UserNotFound
            };
        }

        ClaimsIdentity claimsIdentity = await GetUserClaims(user);

        return new()
        {
            JwtToken = _jwtGenerator.Generate(user.Id,claimsIdentity),
            Result = LoginResult.Success
        };

    }


    //private string GetJwtToken(ClaimsIdentity? claimsIdentity)
    //{
    //    SecurityTokenDescriptor descriptor = new()
    //    {
    //        Issuer = "HRIdentity",
    //        Audience = "HRTicketIdentityUser",
    //        IssuedAt = DateTime.UtcNow,
    //        NotBefore = DateTime.UtcNow.AddMinutes(0),
    //        Expires = DateTime.UtcNow.AddHours(10),
    //        SigningCredentials = CryptoTools.GetJwtCredential(configuration["Jwt:Key"]),
    //        Claims = claimsIdentity.Claims.ToDictionary(c => c.Type, c => (object)c.Value)
    //    };

    //    JwtSecurityTokenHandler tokenHandler = new();

    //    SecurityToken securityToken = tokenHandler.CreateToken(descriptor);

    //    string jwt = tokenHandler.WriteToken(securityToken);

    //    return jwt;
    //}

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