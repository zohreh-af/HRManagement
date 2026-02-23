using HRManagement.Application.Web.Features;

namespace HRManagement.Application.Web;

public interface IClaimManager
{
    Task<List<GetUserRolesByTokenDto>> GetUserRoles();
    Task<bool> IsUserAdmin();
}