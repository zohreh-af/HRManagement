using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlazorHRManagement.Web;

public static partial class Program
{
    public static void Configure(this WebApplication app, IConfiguration configuration)
    {

        app.UseHttpsRedirection();
#if DEBUG
        app.UseDeveloperExceptionPage();
#endif
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }


        app.MapStaticAssets();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();   // required for server-side auth
        app.UseAuthorization();

        app.UseAntiforgery();

        //Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.MapRazorPages();
        app.MapRazorComponents<App>()
           .AddInteractiveServerRenderMode();

        ////app.MapRazorComponents<App>()
        //    .AddInteractiveServerRenderMode()
        //    .RequireAuthorization();
        // app.UseCors();
        //app.UseSession();


    }
}