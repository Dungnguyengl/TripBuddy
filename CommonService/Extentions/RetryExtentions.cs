namespace CommonService.Extentions
{
    public static class RetryExtentions
    {
        public static void Retry(Action func, int times = 3, int wating = 2000)
        {
            ArgumentNullException.ThrowIfNull(func, $"{nameof(RetryExtentions)}:{nameof(Retry)}");

            int runningTime = 0;
            while (runningTime < times)
            {
                try
                {
                    func.Invoke();
                }
                catch
                (Exception e)
                {
                    if (++runningTime == times)
                    {
                        throw;
                    }
                    Task.Delay(runningTime).Wait();
                }
            }
        }

        public static TResult? Retry<TResult>(Func<TResult> func, int times = 3, int wating = 2000)
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
                    if (++runningTime == times)
                    {
                        throw;
                    }
                    Task.Delay(runningTime).Wait();
                }
            }
            return default;
        }
    }
}
