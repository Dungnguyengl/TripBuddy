namespace AspireService.Resources
{
    public sealed class EurekaResource(string name) : ContainerResource(name), IResourceWithConnectionString
    {
        private EndpointReference? _containerEndpoinReference;

        internal const string HttpEndpointName = "container";

        public EndpointReference ContainerEndpointReference => _containerEndpoinReference ??= new(this, HttpEndpointName);

        public ReferenceExpression ConnectionStringExpression => ReferenceExpression.Create($"http://{ContainerEndpointReference.Host}:{ContainerEndpointReference.Property(EndpointProperty.Port)}");

        IEnumerable<object> IValueWithReferences.References => [ConnectionStringExpression];
    }
}
