using BlazorHRManagement.Web.Components;

namespace BlazorHRManagement.Web;

public static partial class Program
{
    public static void Configure(this WebApplication app, IConfiguration configuration)
    {

        app.UseHttpsRedirection();
#if DEBUG
        app.UseDeveloperExceptionPage();
#endif

        app.MapStaticAssets();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();

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
            .AddInteractiveServerRenderMode()
            .RequireAuthorization();
       // app.UseCors();
        //app.UseSession();


    }
}