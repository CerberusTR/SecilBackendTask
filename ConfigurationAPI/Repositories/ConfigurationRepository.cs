using ConfigurationLibrary;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationAPI.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly ConfigurationContext _context;

        public ConfigurationRepository(ConfigurationContext context)
        {
            _context = context;
        }

        public async Task<Configuration> GetConfigurationByNameAsync(string name)
        {
            return await _context.Configurations.FirstOrDefaultAsync(config => config.Name == name);
        }

        public async Task<IEnumerable<Configuration>> GetAllConfigurationsAsync()
        {
            return await _context.Configurations.ToListAsync();
        }

        public async Task AddConfigurationAsync(Configuration config)
        {
            await _context.Configurations.AddAsync(config);
        }

        public async Task UpdateConfigurationAsync(Configuration config)
        {
            _context.Configurations.Update(config);
        }

        public async Task DeleteConfigurationAsync(string name)
        {
            var config = await _context.Configurations.FirstOrDefaultAsync(c => c.Name == name);
            if (config != null)
            {
                _context.Configurations.Remove(config);
            }
        }
    }
}
