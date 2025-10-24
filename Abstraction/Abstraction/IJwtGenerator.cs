using System.Security.Claims;

namespace Abstraction.Abstraction;

public interface IJwtGenerator
{
    string Generate(Guid id, ClaimsIdentity? claimsIdentity);
    int ExpiresInMinutes { get; }
}