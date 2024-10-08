using Microsoft.Extensions.Configuration;

namespace AspireService
{
    public static class ConfigEnviromentVariable
    {
        public static IResourceBuilder<IResourceWithEnvironment> WithEnvironmentByName(this IResourceBuilder<IResourceWithEnvironment> builder, string name, IDistributedApplicationBuilder appBuilder)
        {
            var parameter = appBuilder.Configuration.GetSection(name);

            //builder.ApplicationBuilder.Configuration.AddEnvironmentVariables;

            return builder;
        }
    }
}
