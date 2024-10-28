
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

        public static ApiResponse<T> SuccessResponse(T data, string message = "Request successful.", int status = 200)
        {
            return new ApiResponse<T>(status, message, data);
        }

        public static ApiResponse<T> FailureResponse(string message, int status = 400)
        {
            return new ApiResponse<T>(status, message, default);
        }
        public static ApiResponse<T> FailureResponse500(string message, int status = 500)
        {
            return new ApiResponse<T>(status, message, default);
        }
    }
}
