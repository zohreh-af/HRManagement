using System.Text.Json.Serialization;

namespace HRManagement.Application.Web.Features;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LoginResult
{
    Success,
    UserNotFound,
    Error,
    FailedToLogin,
}