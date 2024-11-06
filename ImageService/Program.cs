
using CommonService.Extentions;

namespace ImageService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(cfg =>
                {
                    cfg.ClearProviders();
                    cfg.AddDebug();
                    cfg.AddConsole();
                })
                .AddTripbuddyConfiguration("image-config-queue")
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
