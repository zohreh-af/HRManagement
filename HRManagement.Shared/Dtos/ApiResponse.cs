namespace HRManagement.Shared.Dtos;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string[] Message { get; set; } 
    public T? Data { get; set; }
    //public IEnumerable<string>? Errors { get; set; }

    //public ApiResponse() { }

    //public ApiResponse(bool success, string message, T? data , IEnumerable<string>? errors = null)
    //{
    //    Success = success;
    //    Message = message;
    //    Data = data;
    //    Errors = errors;
    //}

    //public static ApiResponse<T> SuccessResponse<T>(T data, string message = "Operation successful")
    //{
    //    return new ApiResponse<T>(true, message, data);
    //}

    //public static ApiResponse<T> ErrorResponse<T>(string message = "Operation Failed", IEnumerable<string>? errors = null)
    //{
    //    return new ApiResponse<T>(false, message, default, errors);
    //}
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string[] Message { get; set; }
    public object Data { get; set; }
}