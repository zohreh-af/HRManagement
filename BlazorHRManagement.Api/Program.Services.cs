using Abstraction;
using BlazorHRManagement.Infrastructure.Api.Utilities;
using HRManagement.Application.Api;
using HRManagement.Application.Api.Features;
using HRManagement.Application.Api.Features.Account.Queries.LoginUser;
using HRManagement.Application.Web;
using HRManagement.Domain.Entities;
using HRManagement.Implementation;
using HRManagement.Infrastructure.Api;
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
        var connectionString = configuration.GetConnectionString("SqlDefaultConnectionString");

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
        });
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

        services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazor",
                builder => builder.WithOrigins(
                                   configuration.GetConnectionString("Api:BaseUrl"))
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());
        });
    }
}