using HRManagement.Shared;
using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.Web.Features;

public class LoginUserQuery
{
        [Required(ErrorMessageResourceType = typeof(TextResources), ErrorMessageResourceName = nameof(TextResources.APP_StringKeys_Validation_Required))]
    [Display(ResourceType = typeof(TextResources), Name = nameof(TextResources.APP_StringKeys_Username))]
    public string Username { get; set; }

    [Required(ErrorMessageResourceType = typeof(TextResources), ErrorMessageResourceName = nameof(TextResources.APP_StringKeys_Validation_Required))]
    [Display(ResourceType = typeof(TextResources), Name = nameof(TextResources.APP_StringKeys_Password))]
    public string Password { get; set; }

    public string Ip { get; set; }
}