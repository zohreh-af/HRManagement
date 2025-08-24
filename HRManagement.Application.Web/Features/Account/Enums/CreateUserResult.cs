using System.Text.Json.Serialization;

namespace HRManagement.Application.Web.Features;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CreateUserResult
{
    Success,
    PasswordValidation,
    PasswordAndUsernameDuplicate,
    DuplicateUsername,
    FailedToRegister,
}