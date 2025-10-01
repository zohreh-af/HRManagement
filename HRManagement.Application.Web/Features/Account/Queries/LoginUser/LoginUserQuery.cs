using Abstraction;
using HRManagement.Shared;
using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.Web.Features;

public class LoginUserQuery : IQuery<LoginUserVm>
{
    [Required(ErrorMessageResourceType = typeof(TextResources), ErrorMessageResourceName = nameof(TextResources.APP_StringKeys_Validation_Required))]
    [Display(ResourceType = typeof(TextResources), Name = nameof(TextResources.APP_StringKeys_Email))]
    public string Email { get; set; }

    [Required(ErrorMessageResourceType = typeof(TextResources), ErrorMessageResourceName = nameof(TextResources.APP_StringKeys_Validation_Required))]
    [Display(ResourceType = typeof(TextResources), Name = nameof(TextResources.APP_StringKeys_Password))]
    public string Password { get; set; }

}