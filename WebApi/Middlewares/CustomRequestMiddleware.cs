using CommonService.Services;
using Newtonsoft.Json;

namespace WebApi.Middlewares
{
    public class CustomRequestMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private readonly JsonSerializerSettings _settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
        };

        public async Task Invoke(HttpContext context)
        {
            // Store the original response body stream
            var originalBodyStream = context.Response.Body;
            using var newBodyStream = new MemoryStream();
            context.Response.Body = newBodyStream;
            try
            {
                await _next(context);
                newBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
                newBodyStream.Seek(0, SeekOrigin.Begin);
                var response = JsonConvert.DeserializeObject<Response<object>>(responseBody);
                var odataResponse = JsonConvert.DeserializeObject<ODataRespose<object>>(responseBody);
                context.Response.Clear();
                context.Response.StatusCode = (int?)response?.Code ?? ((int?)odataResponse?.Code) ?? 500;
                if (response?.Content != null)
                { 
                    context.Response.ContentType = "application/json";
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore 
                    };
                    var json = JsonConvert.SerializeObject(response.Content, settings);
                    await context.Response.WriteAsync(json); 
                }
                if (odataResponse?.Value != null)
                {
                    context.Response.ContentType = "application/json";
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    var obj = new
                    {
                        odataResponse.Count,
                        odataResponse.Value
                    };
                    var json = JsonConvert.SerializeObject(obj, settings);
                    await context.Response.WriteAsync(json);
                }
            }
            catch
            {
                newBodyStream.Seek(0, SeekOrigin.Begin);
                await newBodyStream.CopyToAsync(originalBodyStream);
            }

        }
    }
}
