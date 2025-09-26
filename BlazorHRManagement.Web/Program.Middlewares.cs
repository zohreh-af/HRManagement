using BlazorHRManagement.Web.Components;

namespace BlazorHRManagement.Web;

public static partial class Program
{
    public static void Configure(this WebApplication app, IConfiguration configuration)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorComponents<App>()
                    .AddInteractiveServerRenderMode();
        //app.UseCors();

        app.UseHttpsRedirection();

        app.UseSession();

        app.UseAntiforgery();

        app.MapStaticAssets();
    }
}