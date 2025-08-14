using BlazorHRManagement.Infrastructure.Api.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace BlazorHRManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option => {
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    //IssuerSigningKey = new SymmetricSecurityKey()             //you most give it a byte array. 


                    //??? ? ??? ????? ?? ?? ???? ????? ?????? 
                    //????? asymmetric ?? privatekey and for decoding and public key for encoding it
                    RequireExpirationTime= true,
                    ValidateIssuer = true,
                    ValidIssuer = "HRIdentity",
                    ValidateAudience = true,
                    ValidAudience = "HRTicketIdentityUser",
                    IssuerSigningKey = CryptoTools.GetSymmetricKey("L11wA7R4JD2SqlMObNYDXeXtB0tvreWxp5UA7w_XT6E"),

                };
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazor",
                    builder => builder.WithOrigins("https://localhost:7270;http://localhost:5129")
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
