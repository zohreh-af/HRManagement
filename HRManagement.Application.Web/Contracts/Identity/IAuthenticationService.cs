using HRManagement.Application.Web.Features;

namespace HRManagement.Application.Web.Contracts.Identity;

public interface IAuthenticationService
{
    Task<LoginResult> Authenticate(LoginUserQuery query);
}