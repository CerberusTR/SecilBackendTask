using ConfigurationAPI.Repositories;
using ConfigurationLibrary;
using Newtonsoft.Json;

namespace ConfigurationAPI.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfigurationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public async Task<string> GetDataAsync(string data)
        {
            // Implement your data fetching logic here
            // This is just an example, replace it with actual logic
            return await Task.FromResult($"Fetched data for {data}");
        }

        public async Task UpdateConfigurationStatusAsync(string name, bool isActive)
        {
            var config = await _unitOfWork.Configurations.GetConfigurationByNameAsync(name);
            if (config != null)
            {
                config.IsActive = isActive;
                config.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.Configurations.UpdateConfigurationAsync(config);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
