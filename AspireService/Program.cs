using AspireService.Extentions;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// var eureka = builder.AddEurekaServer("eureka", 1234);

// var db = builder.AddConnectionString("mssql");

// var eurekaLocation = eureka.GetEndpoint("http");

var gateway = builder.AddProject<APIGateway>("gateway");

var authentication = builder.AddProject<Authentication>("authen");

var mobileApi = builder.AddProject<MobileApi>("mobile");
    
var webApi = builder.AddProject<WebApi>("web");

var tripper = builder.AddProject<TripperService>("tripper");

var spot = builder.AddProject<SpotService>("spot");

builder.Build().Run();
