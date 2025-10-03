using HRManagement.Identity.Client.Services;
using HRManagement.Presentation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization; // for AuthenticationState
using System;

namespace BlazorHRManagement.Web.Components.Layout;

public partial class BlankLayout : LayoutComponentBase, IDisposable
{
    private string currentTitle = string.Empty;
    public bool IsAutheticated = false;

    [Inject] public AppTitleState TitleState { get; set; } = default!;
    [Inject] public AppAuthenticationStateProvider AuthState { get; set; } = default!;
    [Inject] public NavigationManager Navigation { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        currentTitle = TitleState.Title;
        TitleState.Changed += OnTitleChanged;

        var state = await AuthState.GetAuthenticationStateAsync();
        IsAutheticated = state.User.Identity?.IsAuthenticated == true;

        AuthState.AuthenticationStateChanged += AuthState_AuthenticationStateChanged;
    }

    private void AuthState_AuthenticationStateChanged(Task<AuthenticationState> task)
    {
        IsAutheticated = task.GetAwaiter().GetResult().User.Identity?.IsAuthenticated == true;
        _ = InvokeAsync(StateHasChanged); // ensure we marshal to renderer thread
    }

    private void OnTitleChanged()
    {
        currentTitle = TitleState.Title;
        _ = InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        TitleState.Changed -= OnTitleChanged;
        AuthState.AuthenticationStateChanged -= AuthState_AuthenticationStateChanged;
    }
}
