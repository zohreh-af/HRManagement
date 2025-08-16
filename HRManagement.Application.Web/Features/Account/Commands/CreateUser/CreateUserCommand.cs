using Abstraction;

namespace HRManagement.Application.Web.Features;

public class CreateUserCommand : ICommand<CreateUserVm>
{
    //[Required(ErrorMessageResourceType = typeof(TextResources), ErrorMessageResourceName = nameof(TextResources.APP_StringKeys_Validation_Required))]
    //[Display(ResourceType = typeof(TextResources), Name = nameof(TextResources.APP_StringKeys_Account_Username))]

    public string Username { get; set; }

    //public string Email { get; set; }

    //public bool? EmailConfirmed { get; set; }

    //public DateTime? CreateDate { get; set; }

    //public int? PhoneNumber { get; set; }

    //public bool? PhoneNumberConfirmed { get; set; }

    //public bool IsActive { get; set; }

    //public string CreatorIdentityID { get; set; }

    //public string? LastModifierIdentityID { get; set; }

    //public string Name { get; set; }
    public string Password { get; set; }

    //public string? Details { get; set; }
}