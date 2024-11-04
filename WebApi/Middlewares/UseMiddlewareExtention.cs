namespace WebApi.Middlewares
{
    public static class UseMiddlewareExtention
    {
        public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseCustomeRequest(this IApplicationBuilder builder) 
        {
            return builder.UseMiddleware<CustomRequestMiddleware>();
        }
    }
}
