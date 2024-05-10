using ConfigurationAPI.Repositories;
using ConfigurationLibrary;
using Newtonsoft.Json;

namespace ConfigurationAPI.Services
{
    public class ConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationService(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task<Configuration> GetConfigurationByNameAsync(string name)
        {
            return await _configurationRepository.GetConfigurationByNameAsync(name);
        }

        public async Task<IEnumerable<Configuration>> GetAllConfigurationsAsync()
        {
            return await _configurationRepository.GetAllConfigurationsAsync();
        }

        public async Task AddConfigurationAsync(string name, string type, string value, string applicationName)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || value == null || string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentException("All fields are required.");
            }

            var config = new Configuration
            {
                Name = name,
                Type = type,
                Value = value,
                ApplicationName = applicationName,
                IsActive = false,
                UpdatedDate = DateTime.UtcNow
            };

            await _configurationRepository.AddConfigurationAsync(config);
        }

        public async Task UpdateConfigurationAsync(string name, string type, string value, bool isActive, string applicationName)
        {
            var config = await _configurationRepository.GetConfigurationByNameAsync(name);
            if (config != null)
            {
                config.Type = type;
                config.Value = value;
                config.IsActive = isActive;
                config.ApplicationName = applicationName;
                config.UpdatedDate = DateTime.UtcNow;

                await _configurationRepository.UpdateConfigurationAsync(config);
            }
        }

        public async Task DeleteConfigurationAsync(string name)
        {
            await _configurationRepository.DeleteConfigurationAsync(name);
        }
    }
}
