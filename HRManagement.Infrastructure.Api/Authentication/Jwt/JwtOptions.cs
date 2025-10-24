namespace HRManagement.Infrastructure.Api.Authentication;

public class JwtOptions
{
    public string Secret { get; init; } = default!;
    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public int ExpiresInMinutes { get; init; } = 60;
}