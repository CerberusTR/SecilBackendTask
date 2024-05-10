using ConfigurationLibrary;

namespace ConfigurationAPI.Services
{
    public interface IConfigurationService
    {
        Task<Configuration> GetConfigurationByNameAsync(string name);
        Task<IEnumerable<Configuration>> GetAllConfigurationsAsync();
        Task AddConfigurationAsync(string name, string type, string value, string applicationName);
        Task UpdateConfigurationAsync(string name, string type, string value, bool isActive, string applicationName);
        Task DeleteConfigurationAsync(string name);
        Task<string> GetDataAsync(string data);
        Task UpdateConfigurationStatusAsync(string name, bool isActive);
    }
}