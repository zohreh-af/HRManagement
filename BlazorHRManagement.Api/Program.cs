using BlazorHRManagement.Infrastructure.Api.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


namespace BlazorHRManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

            builder.Services.AddAuthorization();

            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazor",
                    builder => builder.WithOrigins(
                                      "https://localhost:7270;",
                                      "http://localhost:5129")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowBlazor");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
