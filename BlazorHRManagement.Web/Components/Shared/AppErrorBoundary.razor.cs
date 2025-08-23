using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Security.Cryptography;

namespace BlazorHRManagement.Web.Components.Shared;

public partial class AppErrorBoundary
{
    public bool ShowException = false;
    [Inject] public NavigationManager NavigationManager { get; set; }
    ///[Inject] public ILogger<AppErrorBoundary> Logger { get; set; }
    [Inject] public IJSRuntime JSRuntime { get; set; }

#if DEBUG
    protected override async Task OnInitializedAsync()
    {
        ShowException = true;
    }
#endif
    public async Task GoHome(MouseEventArgs e)
    {
        NavigationManager.NavigateTo("/", true);
    }

    protected override async Task OnErrorAsync(Exception ex)
    {
#if DEBUG
        Debugger.Break();
#else
///logger     Logger.LogWarning(ex, ex.Message);
#endif

        if (ex is CryptographicException)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.clear");

            NavigationManager.NavigateTo("/account/login", true);
        }
    }
}