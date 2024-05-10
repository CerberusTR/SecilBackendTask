using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary
{
    public class ConfigurationReader
    {
        private readonly string _applicationName;
        private readonly IConfigRepository _configRepository;
        private readonly IMemoryCache _cache;
        private readonly Timer _refreshTimer;
        private readonly int _refreshInterval;

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _applicationName = applicationName;
            _configRepository = new ConfigRepository(connectionString);
            _cache = new MemoryCache(new MemoryCacheOptions());
            _refreshInterval = refreshTimerIntervalInMs;
            _refreshTimer = new Timer(RefreshConfigurations, null, 0, _refreshInterval);
        }

        private void RefreshConfigurations(object state)
        {
            try
            {
                var configs = _configRepository.GetActiveConfigurations(_applicationName);
                foreach (var config in configs)
                {
                    _cache.Set(config.Name, config.Value);
                }
                Console.WriteLine("Configurations refreshed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing configurations: {ex.Message}");
            }
        }

        public T GetValue<T>(string key)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            throw new KeyNotFoundException($"Key {key} not found in configurations.");
        }
    }
}
