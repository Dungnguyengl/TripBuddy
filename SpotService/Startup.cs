using CommonService.Exceptions;
using CommonService.RPC;
using CommonService.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using SpotService.Controllers.Atraction;
using SpotService.Controllers.Destination;
using SpotService.Controllers.Place;
using SpotService.Controllers.Story;
using SpotService.Model;
using Steeltoe.Discovery.Client;

namespace SpotService
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
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

            services.AddDbContext<EvaluateDbContext>(ops =>
            {
                ops.UseSqlServer(Configuration.GetConnectionString("mssql"));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDiscoveryClient(Configuration);
            services.AddScoped<IInternalService, InternalService>();
            services.AddSingleton<RabbitMQService>();
            services.AddSingleton<RpcClient>();
        }

        public IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<AtractionDTO>("Atraction");
            odataBuilder.EntitySet<DestinationDTO>("Destination");
            odataBuilder.EntitySet<PlaceDTO>("Place");
            odataBuilder.EntitySet<StoryDTO>("Story");
            return odataBuilder.GetEdmModel();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCustomeHandleException();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            HandleChangeConfigurationRuntime(app);
        }

        private void HandleChangeConfigurationRuntime(IApplicationBuilder app)
        {
            ChangeToken.OnChange(Configuration.GetReloadToken, async () =>
            {
                var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
                Console.WriteLine("++++++++++StopByChangeConfiguration++++++++++");
                lifetime.StopApplication();
            });
        }
    }
}
