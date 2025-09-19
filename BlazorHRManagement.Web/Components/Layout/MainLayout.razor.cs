using HRManagement.Infrastructure.Web.Services;
using HRManagement.Presentation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorHRManagement.Web.Components.Layout;

public partial class MainLayout
{
    private string currentTitle = string.Empty;
    [Inject] public AppTitleState TitleState { get; set; }

    protected override async void OnInitializedAsync()
    {
        currentTitle = TitleState.Title;
        TitleState.Changed += OnTitleChanged;
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
