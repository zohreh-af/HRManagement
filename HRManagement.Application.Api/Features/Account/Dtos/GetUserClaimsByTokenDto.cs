namespace HRManagement.Application.Api.Features;

public class GetUserClaimsByTokenDto
{

    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
    public bool IsLinkPinned { get; set; }
    public DateTime PinnedDateTime { get; set; }
}
