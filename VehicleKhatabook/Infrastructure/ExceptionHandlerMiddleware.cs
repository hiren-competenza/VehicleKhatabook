using log4net;
using System.Net;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Infrastructure
{
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }

    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILog logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                // Handle 401 Unauthorized
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    await HandleApiResponseAsync(httpContext, 401, "Unauthorized access - please log in.", null);
                }
                // Handle 403 Forbidden
                else if (httpContext.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    await HandleApiResponseAsync(httpContext, 403, "Access forbidden - you do not have permission.", null);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                await HandleApiResponseAsync(httpContext, 500, "Internal Server Error", null);
            }
        }

        private static Task HandleApiResponseAsync(HttpContext context, int status, string message, object data)
        {
            // Always return 200 OK with an ApiResponse object
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.SuccessResponse(data,message,status);

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
    //public class ExceptionHandlerMiddleware
    //{
    //    private readonly RequestDelegate _next;
    //    private readonly ILog _logger;
    //    public ExceptionHandlerMiddleware(RequestDelegate next, ILog logger)
    //    {
    //        _logger = logger;
    //        _next = next;
    //    }
    //    public async Task InvokeAsync(HttpContext httpContext)
    //    {
    //        try
    //        {
    //            await _next(httpContext);
    //            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
    //            {
    //                await HandleUnauthorizedAsync(httpContext, "Unauthorized access - please log in.");
    //            }
    //            else if (httpContext.Response.StatusCode == (int)HttpStatusCode.Forbidden)
    //            {
    //                await HandleForbiddenAsync(httpContext, "Access forbidden - you do not have permission.");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.Error(ex.Message, ex);
    //            await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
    //        }
    //    }
    //    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    //    {
    //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //        context.Response.ContentType = "application/json";
    //        var errorResponse = ApiResponse<object>.FailureResponse500($"Internal Server Error", context.Response.StatusCode);
    //        var errorJson = System.Text.Json.JsonSerializer.Serialize(errorResponse);
    //        return context.Response.WriteAsync(errorJson);
    //    }
    //    private static Task HandleUnauthorizedAsync(HttpContext context, string message)
    //    {
    //        context.Response.ContentType = "application/json";
    //        var errorResponse = ApiResponse<object>.SuccessResponse(message);
    //        var errorJson = System.Text.Json.JsonSerializer.Serialize(errorResponse);
    //        return context.Response.WriteAsync(errorJson);
    //    }

    //    private static Task HandleForbiddenAsync(HttpContext context, string message)
    //    {
    //        context.Response.ContentType = "application/json";
    //        var errorResponse = ApiResponse<object>.FailureResponse(message, context.Response.StatusCode);
    //        var errorJson = System.Text.Json.JsonSerializer.Serialize(errorResponse);
    //        return context.Response.WriteAsync(errorJson);
    //    }
    //}
}
