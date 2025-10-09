using HRManagement.Identity.Client.Services;
using HRManagement.Presentation;
using Microsoft.AspNetCore.Components;
namespace BlazorHRManagement.Web.Components.Layout;

public partial class MainLayout
{
    private string currentTitle = string.Empty;
    public bool IsAutheticated = false;
    [Inject] public AppTitleState TitleState { get; set; }
    [Inject] public AppAuthenticationStateProvider AuthState { get; set; }
    [Inject] public NavigationManager navigation { get; set; }
    protected override async Task OnInitializedAsync()
    {
        currentTitle = TitleState.Title;
        TitleState.Changed += OnTitleChanged;

        var state = await AuthState.GetAuthenticationStateAsync();

        IsAutheticated = state.User.Identity.IsAuthenticated;

        AuthState.AuthenticationStateChanged += AuthState_AuthenticationStateChanged;
    }
    private void AuthState_AuthenticationStateChanged(Task<Microsoft.AspNetCore.Components.Authorization.AuthenticationState> task)
    {
        IsAutheticated = task.GetAwaiter().GetResult().User.Identity.IsAuthenticated;

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
