using Newtonsoft.Json.Linq;

namespace ConfigurationAPI.Services
{
    public class BackgroundDataFetcher : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundDataFetcher> _logger;
        private Task _executingTask;
        private CancellationTokenSource _cts;

        public BackgroundDataFetcher(IServiceProvider serviceProvider, ILogger<BackgroundDataFetcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _executingTask = ExecuteAsync(_cts.Token);

            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            _cts.Cancel();

            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();
        }

        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BackgroundDataFetcher is starting its work.");
            Console.WriteLine("BackgroundDataFetcher: Veri çekme işlemi başlıyor.");

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
                    if (stoppingToken.IsCancellationRequested)
                    {
                        _logger.LogInformation("BackgroundDataFetcher is stopping its work.");
                        Console.WriteLine("BackgroundDataFetcher: Veri çekme işlemi durduruluyor.");
                        return;
                    }

                    await ProcessConfigurationAsync(item.Key, item.Value, configurationService);
                }
            }

            _logger.LogInformation("BackgroundDataFetcher has completed its work.");
            Console.WriteLine("BackgroundDataFetcher: Veri çekme işlemi tamamlandı.");
        }

        private async Task ProcessConfigurationAsync(string configName, string applicationUrl, IConfigurationService configurationService)
        {
            _logger.LogInformation("Processing configuration: {0}", configName);
            Console.WriteLine($"Veri güncelleme işlemi başlıyor: {configName}");

            try
            {
                var url = $"{applicationUrl}/?name={configName}";
                var data = await configurationService.FetchDataAsync(url);
                _logger.LogInformation("Fetched data for {0}: {1}", configName, data);

                // Parse the fetched data to extract the value
                var parsedData = JArray.Parse(data);
                var newValue = parsedData.FirstOrDefault(item => item["name"]?.ToString() == configName)?["value"]?.ToString();

                // Update the configuration with the new value and set IsActive to true
                var config = await configurationService.GetConfigurationByNameAsync(configName);
                if (config != null && newValue != null)
                {
                    config.Value = newValue;
                    config.IsActive = true;
                    config.UpdatedDate = DateTime.UtcNow;
                    await configurationService.UpdateConfigurationAsync(config.Name, config.Type, config.Value, config.IsActive, config.ApplicationName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to fetch data for {0}: {1}", configName, ex.Message);

                // Set IsActive to false if data fetching fails
                var config = await configurationService.GetConfigurationByNameAsync(configName);
                if (config != null)
                {
                    config.IsActive = false;
                    config.UpdatedDate = DateTime.UtcNow;
                    await configurationService.UpdateConfigurationAsync(config.Name, config.Type, config.Value, config.IsActive, config.ApplicationName);
                }
            }

            Console.WriteLine($"Veri güncelleme işlemi tamamlandı: {configName}");
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }
    }

}
