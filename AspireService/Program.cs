using AspireService.Extentions;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var eureka = builder.AddEurekaServer("eureka", 8761);

var db = builder.AddConnectionString("mssql");

var eurekaLocation = eureka.GetEndpoint("http");

var gateway = builder.AddProject<APIGateway>("gateway")
    .WithReference(eureka);

var authentication = builder.AddProject<Authentication>("authen", "WSL")
    .WithReference(eureka)
    .WithReference(db);

var mobileApi = builder.AddProject<MobileApi>("mobile")
    .WithReference(eureka) 
    .WithReference(db);
    
var webApi = builder.AddProject<WebApi>("web")
    .WithReference(eureka)
    .WithReference(db);

var tripper = builder.AddProject<TripperService>("tripper")
    .WithReference(eureka)
    .WithReference(db);

var spot = builder.AddProject<SpotService>("spot")
    .WithReference(eureka)
    .WithReference(db);

builder.Build().Run();
