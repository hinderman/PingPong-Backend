using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace PingPong_ApiGateway_Api.Middleware
{
    public class ApiMiddleware(ILogger<ApiMiddleware> pLogger) : IMiddleware
    {
        private readonly ILogger<ApiMiddleware> _Logger = pLogger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception pException)
            {
                _Logger.LogError(pException, pException.Message ?? pException.InnerException?.Message);
                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = "Ha ocurrido un error en el servidor"
                };
                string resultJson = JsonSerializer.Serialize(problem);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(resultJson);
            }
        }
    }
}
