namespace ImageService.Services
{
    public class BackgroundJobService(ILogger<BackgroundJobService> logger, S3Service s3Service) : IHostedService, IDisposable
    {
        private readonly ILogger<BackgroundJobService> _logger = logger;
        private readonly S3Service _s3Service = s3Service;
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Job Service is starting.");

            // Schedule the job to run daily at 2 PM UTC+7
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var now = DateTime.Now;
            var scheduledTime = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0, DateTimeKind.Local);
            var initialDelay = scheduledTime - now;

            if (initialDelay < TimeSpan.Zero)
            {
                initialDelay = initialDelay.Add(TimeSpan.FromDays(1));
            }

            _timer = new Timer(DoWork, null, initialDelay, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _logger.LogInformation("Background Job Service is working.");
            try
            {
                _s3Service.DeleteForeverAsync().Wait();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the background job.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Job Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
