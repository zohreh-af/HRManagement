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
        app.UseAuthorization();
        app.MapStaticAssets();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAntiforgery();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.MapRazorPages();
        app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();
    }
}