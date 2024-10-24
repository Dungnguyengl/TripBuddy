using CommonService.Constants;
using Steeltoe.Discovery;

namespace CommonService.Services
{
    public abstract class ServiceBase(HttpClient client, IDiscoveryClient discovery)
    {
        private readonly HttpClient _client = client;
        private readonly IDiscoveryClient _discovery = discovery;
        private const string GATEWAYID = "APIGATEWAY";

        protected HttpClient Client
        {
            get
            {
                var gateway = _discovery.GetInstances(GATEWAYID).FirstOrDefault()
                    ?? throw new ArgumentNullException($"{nameof(ServiceBase)} - {GATEWAYID}");
                _client.BaseAddress ??= gateway.Uri;
                return _client;
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
