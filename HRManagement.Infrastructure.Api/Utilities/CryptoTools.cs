using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlazorHRManagement.Infrastructure.Api.Utilities;

public static class CryptoTools
{

    public static SymmetricSecurityKey GetSymmetricKey(string passKey)
    {
        var key = Encoding.UTF8.GetBytes(passKey);

        return new(key);
    }
}