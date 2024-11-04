using CommonService.Exceptions;
using CommonService.Services;
using Microsoft.Extensions.Primitives;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;
using Steeltoe.Discovery;

namespace APIGateway
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddOcelot(Configuration)
                .AddEureka();
            services.AddScoped<IInternalService, InternalService>();
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
            app.UseOcelot().Wait();
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
