namespace CommonService.Constants
{
    public class ServiceEndpoint
    {
        public ServiceType ServiceType;
        public string PrefixUri;
    }

    public enum ServiceType
    {
        Authentication,
        Spot
    }

    public static class ServiceEndpoints
    {
        public static List<ServiceEndpoint> Internal =
        [
            new() { ServiceType = ServiceType.Authentication, PrefixUri = "auth" },
            new() { ServiceType = ServiceType.Spot, PrefixUri = "spot" }
        ];
    }
}
