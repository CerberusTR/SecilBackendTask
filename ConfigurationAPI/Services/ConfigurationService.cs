using ConfigurationAPI.Repositories;
using ConfigurationLibrary;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ConfigurationAPI.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConfigurationService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ConfigurationService(IUnitOfWork unitOfWork, ILogger<ConfigurationService> logger, IHttpClientFactory httpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Configuration> GetConfigurationByNameAsync(string name)
        {
            return await _unitOfWork.Configurations.GetConfigurationByNameAsync(name);
        }

        public async Task<IEnumerable<Configuration>> GetAllConfigurationsAsync()
        {
            return await _unitOfWork.Configurations.GetAllConfigurationsAsync();
        }

        public async Task AddConfigurationAsync(string name, string type, string value, string applicationName)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || value == null || string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentException("All fields are required.");
            }

            var existingConfig = await _unitOfWork.Configurations.GetConfigurationByNameAsync(name);
            if (existingConfig != null)
            {
                throw new InvalidOperationException("A configuration with the same name already exists.");
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

            await _unitOfWork.Configurations.AddConfigurationAsync(config);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateConfigurationAsync(string name, string type, string value, bool isActive, string applicationName)
        {
            var config = await _unitOfWork.Configurations.GetConfigurationByNameAsync(name);
            if (config != null)
            {
                config.Type = type;
                config.Value = value;
                config.IsActive = isActive;
                config.ApplicationName = applicationName;
                config.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.Configurations.UpdateConfigurationAsync(config);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteConfigurationAsync(string name)
        {
            await _unitOfWork.Configurations.DeleteConfigurationAsync(name);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<string> FetchDataAsync(string url)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data from {Url}", url);
                throw;
            }
        }

        public async Task UpdateConfigurationStatusAsync(string name, bool isActive)
        {
            var config = await _unitOfWork.Configurations.GetConfigurationByNameAsync(name);
            if (config != null)
            {
                _logger.LogInformation("Updating configuration status for {0} to {1}", name, isActive);
                config.IsActive = isActive;
                config.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.Configurations.UpdateConfigurationAsync(config);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                _logger.LogWarning("Configuration {0} not found", name);
            }
        }
    }
}

