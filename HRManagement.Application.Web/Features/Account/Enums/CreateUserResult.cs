namespace HRManagement.Application.Web.Features;

public enum CreateUserResult
{
    Success,
    PasswordValidation,
    PasswordAndUsernameDuplicate,
    DuplicateUsername,
    FailedToRegister,
}