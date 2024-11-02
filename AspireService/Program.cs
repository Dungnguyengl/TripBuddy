using AspireService.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projects;
using System.Xml.Serialization;

var builder = DistributedApplication.CreateBuilder(args);

// var eureka = builder.AddEurekaServer("eureka", 1234);

// var db = builder.AddConnectionString("mssql");

// var eurekaLocation = eureka.GetEndpoint("http");
List<IResourceBuilder<ProjectResource>> resourceBuilders =
    [
        builder.AddProject<APIGateway>("APIGATEWAY"),
        builder.AddProject<Authentication>("AUTHENTICATION"),
        builder.AddProject<MobileApi>("MOBILEAPI"),
        builder.AddProject<WebApi>("WEBAPI"),
        builder.AddProject<TripperService>("TRIPPERSERVICE"),
        builder.AddProject<SpotService>("SPOTSERVICE")
    ];

resourceBuilders.ForEach(x => x.WithReplicas(10));

var app = builder.Build();

var services = app.Services;
var resource = services.GetServices<ProjectResource>();


IHostApplicationLifetime? lifetime = services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopped.Register(() =>
{
    using var client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:8761/eureka/apps/");
    var serializer = new XmlSerializer(typeof(EurekaNode));

    var task = builder.Resources.Where(x => x.GetType() == typeof(ProjectResource))
    .Select(resource =>
    {

        return Task.Run(async () =>
        {
            var res = await client.GetAsync(resource.Name);
            if (!res.IsSuccessStatusCode)
            {
                Console.WriteLine($"[FAIL]-{resource.Name}-{res.StatusCode}");
                return;
            }
            var rawXml = await res.Content.ReadAsStreamAsync();
            var xml = (EurekaNode?)serializer.Deserialize(rawXml);

            var tasks = xml?.EurekaInstances.Select(instance =>
            {
                return Task.Run(async () => { 
                    //Console.WriteLine($"DELETING - {resource.Name}/{instance.InstanceId}");
                    var res = await client.DeleteAsync($"{resource.Name}/{instance.InstanceId}"); 
                    Console.WriteLine($"DELETED - {resource.Name}/{instance.InstanceId} - {(res.IsSuccessStatusCode ? "Success" : "Fail")} ");
                });
            });

            await Task.WhenAll(tasks?.ToArray() ?? []);
        });
    });

    Task.WhenAll(task.ToArray() ?? []).Wait();
});

app.Run();
