using Blazored.LocalStorage;
using HRManagement.Infrastructure.Web.Utilities;
using HRManagement.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace HRManagement.Infrastructure.Web.Services;

public partial class ApiHandler(IConfiguration configuration
        , ILocalStorageService localStorage
        , ILogger<ApiHandler> logger
        , NavigationManager navigationManager) : HttpClientHandler
{
    private string baseUri;

    public async Task<ApiResponse<T>> SendAsyncObjectByUri<T>(HttpMethod method, string uri, object data = null)
    {
        baseUri = baseUri = configuration["Api:BaseUrl"]?.Trim();
       
        if (string.IsNullOrWhiteSpace(baseUri))
            throw new InvalidOperationException("Api:BaseUrl is not configured.");
       
        JsonSerializerOptions option = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            //Converters =
            //{
            //    new NullableDateTimeConverter()
            //}
        };


        if (method == HttpMethod.Get || data is null)
            throw new InvalidOperationException($"Invalid Get Request or Null Data");

        var passedDataJsonString = JsonSerializer.Serialize(data, option);
        var buffer = Encoding.UTF8.GetBytes(passedDataJsonString);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpRequestMessage request = new(method, baseUri + uri);

        request.Content = byteContent;

        var response = await SendAsync(request, new CancellationToken());

        var mediaType = response.Content?.Headers?.ContentType?.MediaType;

        var body = response.Content is null ? null : await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (string.Equals(mediaType, "application/json", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(body))
            {
                var err = JsonSerializer.Deserialize<ApiResponse<string>>(body, option);
                throw new HttpRequestException($"HTTP {(int)response.StatusCode} {response.ReasonPhrase}. Server says: {string.Join(" | ", err?.Message ?? Array.Empty<string>())}");
            }
            else
            {
                throw new HttpRequestException(
                    $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}. Expected JSON but got '{mediaType ?? "no content"}'. Body: {Truncate(body, 200)}");
            }
        }
        if (!string.Equals(mediaType, "application/json", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(body))
            throw new InvalidOperationException($"Expected JSON response but got '{mediaType ?? "no content"}'. Body: {Truncate(body, 200)}");


        var result = JsonSerializer.Deserialize<ApiResponse<T>>(body, option)
                 ?? throw new InvalidOperationException("Failed to deserialize server response.");
        return result;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await ManageHeader(request);

        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            if (response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse<string>>(cancellationToken: cancellationToken);
                logger.LogWarning($"Auth log error: {errorResult}");
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    localStorage.RemoveItemAsync(SessionStorageKeys.AuthToken, cancellationToken);
                    localStorage.RemoveItemAsync(SessionStorageKeys.UserAlias, cancellationToken);
                    localStorage.RemoveItemAsync(SessionStorageKeys.SecureToken, cancellationToken);
                    localStorage.RemoveItemAsync(SessionStorageKeys.SessionStart, cancellationToken);

                    navigationManager.NavigateTo("/account/login", true);
                }
                //if (response.StatusCode == HttpStatusCode.Ambiguous)
                //{
                //    navigationManager.NavigateTo("/settings/apisettings");
                //}
            }
        }
        return response;
    }

    private async Task ManageHeader(HttpRequestMessage request)
    {
        var token = await localStorage.GetItemAsync<string>(SessionStorageKeys.AuthToken);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new("Bearer", token);
        }
        else
        {
            request.Headers.Remove("Authorization");
        }
    }
    private static string Truncate(string? s, int max) =>
    string.IsNullOrEmpty(s) ? "" : (s.Length <= max ? s : s.Substring(0, max));
}