using Microsoft.AspNetCore.Builder;

namespace CommonService.Exceptions
{
    public static class UseMiddlewareExtention
    {
        public static IApplicationBuilder UseCustomeHandleException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HandleExceptionMiddleware>();
        }
    }
}
