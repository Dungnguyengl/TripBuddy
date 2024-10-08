using CommonService.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using SpotService.Model;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System.Text;
using System.Text.Json;
using Steeltoe.Discovery.Client.SimpleClients;
using SpotService.Controllers.Atraction;

namespace SpotService
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            //ConfigFromExternal(services);

            services.AddControllers()
                .AddOData(opt =>
                {
                    opt.AddRouteComponents("api", GetEdmModel())
                        .Select()
                        .Filter()
                        .OrderBy()
                        .Expand()
                        .Count()
                        .SetMaxTop(100);
                });

            services.AddDbContext<SpotDbContext>(ops =>
            {
                ops.UseSqlServer(Configuration.GetConnectionString("mssql"));
            });

            //services.AddServiceDiscovery(ops =>
            //{
            //    var e = ops.Extensions;
            //    ops.UseEureka();
            //});

            services.AddHttpClient("APIGATEWAY")
                .AddServiceDiscovery()
                .AddTypedClient<IInternalService, InternalService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<AtractionDTO>("Atraction");
            return odataBuilder.GetEdmModel();
        }

        public void ConfigFromExternal ()
        {
            var factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("eureka", true, false, false, null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var json = JsonSerializer.Deserialize<Dictionary<string, string>>(message);

                var builder = new ConfigurationBuilder();
                var configuation = builder.AddInMemoryCollection(json).Build();

                //using var services = new ServiceProvider();
                //services.AddServiceDiscovery(ops =>
                //{
                //    configuation.Bind(ops);
                //    ops.UseEureka(cfg =>
                //    {
                        
                //    });
                //});


            };
            channel.BasicConsume("eureka", true, consumer);
        } 

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
