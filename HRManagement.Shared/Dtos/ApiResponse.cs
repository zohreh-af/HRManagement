using System.Text.Json.Serialization;

namespace HRManagement.Shared.Dtos;

public class ApiResponse<T>
{
    [JsonPropertyName("successful")]
    public bool Successful { get; set; }

    [JsonPropertyName("messages")]
    public string[] Message { get; set; }

    [JsonPropertyName("value")]
    public T? Data { get; set; }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string[] Message { get; set; }
    public object Data { get; set; }
}

public class ApiRequest
{
    /// <summary>
    /// Should be: RestAPI
    /// </summary>
    [JsonPropertyName("interface")]
    public string Interface { get; set; }

    /// <summary>
    /// Name of your method in current project
    /// </summary>
    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("parameters")]
    public object Parameters { get; set; }
}