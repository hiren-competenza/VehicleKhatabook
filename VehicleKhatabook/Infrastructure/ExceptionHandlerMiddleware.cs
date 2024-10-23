using log4net;
using System.Net;
using VehicleKhatabook.Entities.Models;
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
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var errorResponse = ApiResponse<object>.FailureResponse500($"An error occurred: {exception.Message}");
            var errorJson = System.Text.Json.JsonSerializer.Serialize(errorResponse);
            return context.Response.WriteAsync(errorJson);
        }
    }
}
