using Abstraction.Abstraction;
using BlazorHRManagement.Infrastructure.Api.Utilities;
using HRManagement.Application.Api;
using HRManagement.Implementation;
using HRManagement.Infrastructure.Api;
using HRManagement.Infrastructure.Api.Authentication;
using HRManagement.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text.Json.Serialization;

namespace BlazorHRManagement.Api;

public static partial class Program
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var apiBaseUrl = configuration["Api:BaseUrl"];

        var connectionString = configuration.GetConnectionString("SqlDefaultConnectionString");
        
        services.AddIdentityCore<User>();

        services.AddScoped<IJwtGenerator, JwtGenerator>();

        services.AddControllers().AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(option =>
        {
            option.RequireHttpsMetadata = false;
            option.SaveToken = true;

            option.TokenValidationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),
                ValidateIssuer = true,
                ValidIssuer = "HRIdentity",
                ValidateAudience = true,
                ValidAudience = "HRTicketIdentityUser",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = CryptoTools.GetSymmetricKey("L11wA7R4JD2SqlMObNYDXeXtB0tvreWxp5UA7w_XT6E"),
            };
            option.Events = new JwtBearerEvents
            {
                OnMessageReceived = ctx =>
                {
                    if (string.IsNullOrEmpty(ctx.Token) &&
                        ctx.HttpContext.Request.Cookies.TryGetValue("access_token", out var cookie))
                    {
                        ctx.Token = cookie;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddInfrastructureApiServices(configuration);
        services.AddApplicationApiServices();
        services.AddImplementationServices();

        services.AddDbContext<HRManagementContext>(option =>
        {
            option.UseSqlServer(connectionString);
        });

        services.AddAuthorization();
        services.AddOpenApi();

        services.AddCors(o =>
        {
            o.AddPolicy("AllowWeb",
                p => p.WithOrigins("https://localhost:7270", "http://localhost:5129")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials());
        });
    }
}