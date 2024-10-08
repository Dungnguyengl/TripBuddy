using CommonService.Constants;
using Steeltoe.Discovery;

namespace CommonService.Services
{
    public abstract class ServiceBase
    {
        private readonly HttpClient _client;
        private readonly IDiscoveryClient _discovery;
        private const string GATEWAYID = "APIGATEWAY";

        protected HttpClient Client
        {
            get
            {
                var gateway = _discovery.GetInstances(GATEWAYID).FirstOrDefault() ?? throw new Exception($"{nameof(ServiceBase)} - {GATEWAYID} not found");
                _client.BaseAddress = gateway.Uri;
                return _client;
            }
        }

        protected string GetEndPoint(ServiceType type, string path = "")
        {
            var serviceEndpoint = ServiceEndpoints.Internal.Find(x => x.ServiceType == type) ?? throw new Exception($"{nameof(ServiceBase)}-{nameof(GetEndPoint)} Not found service");
            return !string.IsNullOrEmpty(path) ? $"{serviceEndpoint.PrefixUri}/{path}" : serviceEndpoint.PrefixUri;
        }

        public ServiceBase(HttpClient client, IDiscoveryClient discovery)
        {
            _client = client;
            _discovery = discovery;
        }
    }
}
