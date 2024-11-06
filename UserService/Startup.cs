using CommonService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using UserService.Models;
using UserService.Services;

namespace UserService
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserServiceDbContext>(ops =>
            {
                ops.UseSqlServer(Configuration.GetConnectionString("mssql"));
            });

            services.AddControllers();

            services.AddSingleton<RabbitMQService>();
            services.AddSingleton<UserRpcService>();
            services.AddScoped<HandleUserService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var servces = scope.ServiceProvider;
            //servces.GetRequiredService<ImageRpcService>();

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
