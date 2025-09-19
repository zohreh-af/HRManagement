using HRManagement.Presentation;
using Microsoft.AspNetCore.Components;

namespace BlazorHRManagement.UI.Components.Shared;

public class TitleSync
{
    [Parameter] public string Value { get; set; } = string.Empty;
    [Inject] public AppTitleState TitleState { get; set; }

    protected override void OnParametersSet()
    {
        TitleState.Set(Value);
    }
}
