using Amazon.S3;
using CommonService.Services;
using ImageService.Models;
using ImageService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace ImageService
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAWSService<IAmazonS3>();
            services.AddSingleton<S3Service>();
            services.AddSingleton<RabbitMQService>();
            services.AddSingleton<FileReceiverService>();
            services.AddSingleton<ImageRpcService>();
            services.AddHostedService<BackgroundJobService>();
            services.AddDbContext<ImageServiceDbContext>(ops =>
            {
                ops.UseSqlServer(Configuration.GetConnectionString("mssql"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var servces = scope.ServiceProvider;
            var s3 = servces.GetRequiredService<S3Service>();
            servces.GetRequiredService<FileReceiverService>().FileCompleted += async (streamFile, key) => await s3.StoreImageAsync(streamFile, key);
            servces.GetRequiredService<ImageRpcService>();

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
