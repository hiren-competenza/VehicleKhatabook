
namespace VehicleKhatabook.Models.Common
{
    public class ApiResponse<T>
    {
        public int status { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(int status, string message, T data)
        {
            this.status = status;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Request successful.")
        {
            return new ApiResponse<T>(200, message, data);
        }

        public static ApiResponse<T> FailureResponse(string message)
        {
            return new ApiResponse<T>(200, message, default);
        }
        public static ApiResponse<T> FailureResponse500(string message)
        {
            return new ApiResponse<T>(500, message, default);
        }
    }
}
