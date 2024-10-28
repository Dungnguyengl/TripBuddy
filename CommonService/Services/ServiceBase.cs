using CommonService.Constants;
using CommonService.Extentions;
using Microsoft.AspNetCore.Http;
using Steeltoe.Discovery;

namespace CommonService.Services
{
    public abstract class ServiceBase(IHttpClientFactory clientFactory, IDiscoveryClient discovery, IHttpContextAccessor context)
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly IDiscoveryClient _discovery = discovery;
        private readonly HttpContext? _context = context.HttpContext;
        private const string GATEWAYID = "APIGATEWAY";

        protected HttpClient Client
        {
            get
            {
                var gateway = _discovery.GetInstances(GATEWAYID).FirstOrDefault()
                    ?? throw new ArgumentNullException($"{nameof(ServiceBase)} - {GATEWAYID}");
                var client = _clientFactory.CreateClient();
                var token = _context?.Request.Headers.Authorization.FirstOrDefault(x => !x.IsNullOrEmpty() && x.StartsWith("Bearer"))
                    ?.Replace("Bearer ", "") ?? string.Empty;
                client.BaseAddress ??= gateway.Uri;
                if (!token.IsNullOrEmpty())
                {
                    client.DefaultRequestHeaders.Authorization = new("Bearer", token);
                }
                return client;
            }
        }

        protected string GetEndPoint(ServiceType type, string path = "")
        {
            var serviceEndpoint = ServiceEndpoints.Internal.Find(x => x.ServiceType == type)
                ?? throw new ArgumentNullException($"{nameof(ServiceBase)}-{nameof(GetEndPoint)}");
            return !string.IsNullOrEmpty(path) ? $"{serviceEndpoint.PrefixUri}/{path}" : serviceEndpoint.PrefixUri;
        }
    }
}
