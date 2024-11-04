using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommonService.Exceptions
{
    public class HandleExceptionMiddleware(RequestDelegate next, ILogger<HandleExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger _logger = logger;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (NotFoundException e)
            {
                httpContext.Response.StatusCode = 404;
                var body = new { e.Message };
                await httpContext.Response.WriteAsJsonAsync(body);
                _logger.Log(LogLevel.Error, "{message}", e.Message);
            }
            catch (BadRequestException e)
            {
                httpContext.Response.StatusCode = 400;
                var body = new { e.Message };
                await httpContext.Response.WriteAsJsonAsync(body);
                _logger.Log(LogLevel.Error, "{message}", e.Message);
            }
            catch (Exception e)
            {
                httpContext.Response.StatusCode = 500;
                var body = new { e.Message };
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(body));
                _logger.LogError(e, "An unexpected error occurred");
            }
        }
    }
}
