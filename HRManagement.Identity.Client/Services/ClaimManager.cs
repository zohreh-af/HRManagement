using HRManagement.Application.Api.Features;
using HRManagement.Application.Web;
using HRManagement.Application.Web.Features;
using HRManagement.Infrastructure.Web.Services;

namespace HRManagement.Identity.Client;

public partial class ClaimManager(ApiHandler Api) : IClaimManager
{
    private List<GetUserClaimsByTokenDto> claims;
    private List<GetUserRolesByTokenDto> roles;
    private bool? isAdmin = null;

    //public async Task<List<GetUserClaimsByTokenDto>> GetUserClaims(bool forceReload = false)
    //{
    //    //if (claims is null || forceReload)
    //    //{
    //    //    claims = (await Api.SendAsyncObjectByUri<GetUserClaimsByTokenVm>(HttpMethod.Get, "Account/GetUserClaimsByToken")).Value.Data;
    //    //}

    //    //return claims;

    //}

    public async Task<List<GetUserRolesByTokenDto>> GetUserRoles()
    {
        if (roles is null)
        {
            //roles = (await Api.SendAsyncObjectByUri<GetUserRolesByTokenVm>(HttpMethod.Get, "Account/GetUserRolesByToken")).Value.Roles;
        }

        return roles;
    }

    public async Task ClearDataLists()
    {
        claims = null;

        roles = null;

        isAdmin = null;
    }

    public async Task<bool> IsUserAdmin()
    {
        if (isAdmin is null)
        {
            var localRoles = (await GetUserRoles());

            if (localRoles is not null)
            {
                isAdmin = localRoles.Any(p => p.Name.ToLower() == "admin");
            }
            else
            {
                return false;
            }
        }

        return (bool)isAdmin;
    }
}