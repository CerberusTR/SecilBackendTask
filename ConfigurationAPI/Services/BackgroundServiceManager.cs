namespace ConfigurationAPI.Services
{
    public class BackgroundServiceManager
    {
        private readonly IHostedService _hostedService;

        public BackgroundServiceManager(IHostedService hostedService)
        {
            _hostedService = hostedService;
        }

        public async Task StartAsync()
        {
            await _hostedService.StartAsync(default);
        }

        public async Task StopAsync()
        {
            await _hostedService.StopAsync(default);
        }
    }
}
