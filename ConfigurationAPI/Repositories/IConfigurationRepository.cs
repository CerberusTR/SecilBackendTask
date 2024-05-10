using ConfigurationLibrary;

namespace ConfigurationAPI.Repositories
{
    public interface IConfigurationRepository
    {
        Task<Configuration> GetConfigurationByNameAsync(string name);
        Task<IEnumerable<Configuration>> GetAllConfigurationsAsync();
        Task AddConfigurationAsync(Configuration config);
        Task UpdateConfigurationAsync(Configuration config);
        Task DeleteConfigurationAsync(string name);
    }
}
