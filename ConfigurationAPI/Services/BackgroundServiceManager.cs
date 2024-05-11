namespace ConfigurationAPI.Services
{
    public class BackgroundServiceManager
    {
        private readonly BackgroundDataFetcher _backgroundDataFetcher;

        public BackgroundServiceManager(IEnumerable<IHostedService> hostedServices)
        {
            _backgroundDataFetcher = hostedServices.OfType<BackgroundDataFetcher>().FirstOrDefault();
        }

        public Task StartAsync()
        {
            if (_backgroundDataFetcher != null)
            {
                return _backgroundDataFetcher.StartAsync(CancellationToken.None);
            }
            throw new InvalidOperationException("BackgroundDataFetcher service is not available.");
        }

        public Task StopAsync()
        {
            if (_backgroundDataFetcher != null)
            {
                return _backgroundDataFetcher.StopAsync(CancellationToken.None);
            }
            throw new InvalidOperationException("BackgroundDataFetcher service is not available.");
        }
    }
}
