namespace Backend.Api.Common
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(T data, string message = "")
        {
            IsSuccess = true;
            Data = data;
            Message = message;
        }

        public ApiResponse(string message, List<string>? errors = null)
        {
            IsSuccess = false;
            Message = message;
            Errors = errors;
        }

        public static ApiResponse<T> Success(T data, string message = "")
        {
            return new ApiResponse<T>(data, message);
        }

        public static ApiResponse<T> Failure(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>(message, errors);
        }
    }

    // For non-generic responses
    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse() : base() { }
        public ApiResponse(string message, List<string>? errors = null) : base(message, errors) { }

        public static ApiResponse Success(string message = "")
        {
            return new ApiResponse { IsSuccess = true, Message = message };
        }

        public static new ApiResponse Failure(string message, List<string>? errors = null)
        {
            return new ApiResponse(message, errors);
        }
    }
}