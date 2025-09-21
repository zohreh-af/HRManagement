using HRManagement.Identity.Client.Services;
using HRManagement.Infrastructure.Web.Services;
using HRManagement.Presentation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorHRManagement.Web.Components.Layout;

public partial class MainLayout
{
    private string currentTitle = string.Empty;
    public bool IsAuthenticated = false;
    [Inject] public AppTitleState TitleState { get; set; }
    [Inject] public AppAuthenticationStateProvider AuthState { get; set; }
    [Inject] public NavigationManager navigation { get; set; }
    protected override async Task OnInitializedAsync()
    {
        currentTitle = TitleState.Title;
        TitleState.Changed += OnTitleChanged;

        var state = await AuthState.GetAuthenticationStateAsync();
        if (state.User.Identity.IsAuthenticated)
        {
            IsAuthenticated =true;
        }
    }
    private void AuthState_AuthenticationStateChanged(Task<Microsoft.AspNetCore.Components.Authorization.AuthenticationState> task)
    {
        IsAuthenticated = task.GetAwaiter().GetResult().User.Identity.IsAuthenticated;

        StateHasChanged();
    }

    private void OnTitleChanged()
    {
        currentTitle = TitleState.Title;
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        TitleState.Changed -= OnTitleChanged;
    }
}
