namespace ConfigurationAPI.Services
{
    public class BackgroundDataFetcher : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
        private readonly HttpClient _httpClient;

        public BackgroundDataFetcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _httpClient = new HttpClient();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(3));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var configurationService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
                var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();

                var configurations = await configurationService.GetAllConfigurationsAsync();
                var applications = await applicationService.GetAllApplicationsAsync();

                var configAppDictionary = (from config in configurations
                                           join app in applications
                                           on config.ApplicationName equals app.ServiceName
                                           select new { config.Name, app.ApplicationUrl })
                                           .ToDictionary(item => item.Name, item => item.ApplicationUrl);

                foreach (var item in configAppDictionary)
                {
                    await ProcessConfigurationAsync(item.Key, item.Value, configurationService);
                }
            }
        }

        private async Task ProcessConfigurationAsync(string configName, string applicationUrl, IConfigurationService configurationService)
        {
            Console.WriteLine("Processing configuration: {0}", configName);

            bool isActive = await CheckServiceAsync(applicationUrl, configName);
            await configurationService.UpdateConfigurationStatusAsync(configName, isActive);
        }

        private async Task<bool> CheckServiceAsync(string applicationUrl, string configName)
        {
            try
            {
                var url = $"{applicationUrl}/?name={configName}";
                var response = await _httpClient.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
