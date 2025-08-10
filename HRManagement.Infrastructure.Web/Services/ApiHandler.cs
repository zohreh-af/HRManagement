using Blazored.LocalStorage;
using HRManagement.Infrastructure.Web.Utilities;
using HRManagement.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Buffers.Text;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
namespace HRManagement.Infrastructure.Web.Services;

public partial class ApiHandler(IConfiguration configuration
        , ILocalStorageService localStorage
        , ILogger<ApiHandler> logger
        , NavigationManager navigationManager  ) : HttpClientHandler
{
    private string baseUri;

    public async Task<ApiResponse<T>>SendAsyncObjectByUri<T>(HttpMethod method, string uri,object data = null)
    {
        if (!string.IsNullOrEmpty(baseUri))
        {
            baseUri = configuration["Api:BaseUrl"]; //if baseUri was empty, fetch data from configuration
        }
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
        var passedDataJsonString = JsonSerializer.Serialize(data, option);
        var buffer = Encoding.UTF8.GetBytes( passedDataJsonString );
        var byteContent = new ByteArrayContent( buffer );
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json" );

        HttpRequestMessage request = new(method, baseUri + uri);

        request.Content = byteContent;
        
        var resultStream = await SendAsync(request,new CancellationToken());

        Stream httpStream = await resultStream.Content.ReadAsStreamAsync();
        //using StreamReader sr = new (httpStream);   

        var result = await JsonSerializer.DeserializeAsync<ApiResponse<T>>(httpStream, option);
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
        var storageAuthResult = await localStorage.SetItemAsync<string>(SessionStorageKeys.AuthToken);

        if (storageAuthResult.Success)
        {
            request.Headers.Authorization = new("Bearer", storageAuthResult.Value);
        }
        else
        {
            request.Headers.Remove("Authorization");
        }
    }
}