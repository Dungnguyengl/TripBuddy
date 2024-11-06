using CommonService.Exceptions;
using CommonService.Services;
using Microsoft.Extensions.Primitives;
using Steeltoe.Discovery.Client;
using WebApi.Middlewares;

namespace WebApi
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddDiscoveryClient(Configuration);
            services.AddScoped<IInternalService, InternalService>();

            services.AddControllers()
                .AddNewtonsoftJson(cfg =>
                {

                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowSpecificOrigins",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowSpecificOrigins");

            app.UseCustomeHandleException();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCustomAuthentication();
            app.UseCustomeRequest();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            HandleChangeConfigurationRuntime(app);
        }

        private void HandleChangeConfigurationRuntime(IApplicationBuilder app)
        {
            ChangeToken.OnChange(Configuration.GetReloadToken, () =>
            {
                var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
                Console.WriteLine("++++++++++StopByChangeConfiguration++++++++++");
                lifetime.StopApplication();
            });
        }
    }
}
