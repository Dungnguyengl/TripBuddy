namespace CommonService.Constants
{
    public class ServiceEndpoint
    {
        public ServiceType ServiceType;
        public string PrefixUri;
    }

    public enum ServiceType
    {
        Authentication
    }

    public static class ServiceEndpoints
    {
        public static List<ServiceEndpoint> Internal =
        [
            new() { ServiceType = ServiceType.Authentication, PrefixUri = "auth" }
        ];
    }
}
