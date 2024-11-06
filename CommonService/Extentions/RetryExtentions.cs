using Microsoft.Extensions.Logging;

namespace CommonService.Extentions
{
    public static class RetryExtentions
    {
        public static void Retry(Action func, int times = 3, int wating = 2000, ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func, $"{nameof(RetryExtentions)}:{nameof(Retry)}");

            int runningTime = 0;
            while (runningTime < times)
            {
                try
                {
                    func.Invoke();
                    return;
                }
                catch
                (Exception e)
                {
                    if (++runningTime == times)
                    {
                        logger?.LogWarning(e, "Occus Excption in the {time} times", runningTime);
                        throw;
                    }
                    Task.Delay(runningTime).Wait();
                }
            }
        }

        public static TResult Retry<TResult>(Func<TResult> func, int times = 3, int wating = 2000, ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func, $"{nameof(RetryExtentions)}:{nameof(Retry)}");

            int runningTime = 0;
            while (runningTime < times)
            {
                try
                {
                    return func.Invoke();
                }
                catch
                (Exception e)
                {
                    if (++runningTime < times)
                    {
                        logger?.LogWarning(e, "Occus Excption in the {time} times", runningTime);
                        Task.Delay(wating).Wait();
                        continue;
                    }
                }
            }
            return func.Invoke();
        }
    }
}
