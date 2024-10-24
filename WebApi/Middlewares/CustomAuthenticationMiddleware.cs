using CommonService.Services;
using System.Net;

namespace WebApi.Middlewares
{
    public class CustomAuthenticationMiddleware(RequestDelegate next, IInternalService internalService)
    {
        private readonly RequestDelegate _next = next;
        private readonly IInternalService _intenalService = internalService;

        public async Task Invoke(HttpContext context)
        {
            var route = context.Request.Path;

            if (route.HasValue && route.Value.StartsWith("/api/Authen"))
            {
                await _next(context);
                return;
            }

            var tasks = context.Request.Headers.Authorization.Select(token =>
            {
                var param = new { AccessToken = token.Replace("Bearer ", "") };

                return _intenalService.PostAsync<RequestData>(CommonService.Constants.ServiceType.Authentication, param, "token/isValid");
            });

            var results = await Task.WhenAll(tasks);
            if (!results.Any(res => res.Code == HttpStatusCode.OK))
            {
                context.Response.StatusCode = 401;
                return;
            }

            await _next(context);
        }
    }

    internal class RequestData
    {
        public string Message { get; set; }
    }
}
