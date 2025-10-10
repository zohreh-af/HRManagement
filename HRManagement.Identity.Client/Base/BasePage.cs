using HRManagement.Identity.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace HRManagement.Identity.Client.Base;

public class BasePage : ComponentBase
{
    [Inject] public AppAuthenticationStateProvider AuthState { get; set; } = default!;
    [Inject] public NavigationManager Nav { get; set; } = default!;

    private static readonly HashSet<string> AllowedAnonymous = new(StringComparer.OrdinalIgnoreCase)
    {
        "/account/login"
    };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var state = await AuthState.GetAuthenticationStateAsync();

        // اگر لاگین نیست، فقط صفحات مجاز ناشناس را ببیند
        if (!state.User.Identity?.IsAuthenticated ?? true)
        {
            if (!IsAllowedForAnonymous(CurrentPath()))
                Nav.NavigateTo("/account/login", forceLoad: true);
            return;
        }

        // در صورت نیاز: اینجا بررسی دسترسی‌های کاربر (Claims/Roles) را بگذار
        // if (!UserHasAccess(state.User, CurrentPath())) Nav.NavigateTo("/account/login", true);
    }

    private string CurrentPath()
    {
        // "account/login?x=1#y" -> "/account/login"
        var rel = Nav.ToBaseRelativePath(Nav.Uri);
        var path = "/" + rel.Split('?', '#')[0].Trim('/'); // همیشه با / شروع شود
        return string.IsNullOrEmpty(path) ? "/" : path.ToLowerInvariant();
    }

    private static bool IsAllowedForAnonymous(string path) => AllowedAnonymous.Contains(path);

    // نمونه برای آینده:
    // private static bool UserHasAccess(ClaimsPrincipal user, string path) { ... }
}
