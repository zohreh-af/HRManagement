//using Microsoft.AspNetCore.Http;
//using System.Net.Http.Headers;

//namespace HRManagement.Infrastructure.Api;

//public class AuthHeaderHandler : DelegatingHandler
//{
//    private readonly IHttpContextAccessor _httpContextAccessor;

//    public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
//    {
//        _httpContextAccessor = httpContextAccessor;
//    }

//    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    {
//        var token = _httpContextAccessor.HttpContext?.Session?.GetString("JWT");
//        if (!string.IsNullOrEmpty(token))
//        {
//            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//        }

//        return base.SendAsync(request, cancellationToken);
//    }
//}