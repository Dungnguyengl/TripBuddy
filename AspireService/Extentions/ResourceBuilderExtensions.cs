using AspireService.Resources;

namespace AspireService.Extentions
{
    public static class ResourceBuilderExtensions
    {
        public static IResourceBuilder<EurekaResource> AddEurekaServer(this IDistributedApplicationBuilder builder, string name = "eureka-server", int? port = null)
        {
            var resource = new EurekaResource(name);

            return builder.AddResource(resource)
                .WithImage(EurekaContainerImageTags.Image)
                .WithImageTag(EurekaContainerImageTags.Tag)
                .WithHttpEndpoint(
                targetPort: 8761,
                port: port,
                name: EurekaResource.HttpEndpointName);
        }
    }

    internal static class EurekaContainerImageTags
    {
        internal const string Registry = "";

        internal const string Image = "eureka-server";

        internal const string Tag = "latest";
    }
}
