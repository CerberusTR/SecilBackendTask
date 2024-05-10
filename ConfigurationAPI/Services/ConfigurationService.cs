using ConfigurationLibrary;
using Newtonsoft.Json;

namespace ConfigurationAPI.Services
{
    public class ConfigurationService
    {
        private readonly ConfigurationContext _context;

        public ConfigurationService(ConfigurationContext context)
        {
            _context = context;
        }

        public Configuration GetConfigurationByName(string name)
        {
            return _context.Configurations.FirstOrDefault(config => config.Name == name);
        }

        public IEnumerable<Configuration> GetAllConfigurations()
        {
            return _context.Configurations.ToList();
        }

        public void AddConfiguration(string name, string type, string value, string applicationName)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || value == null || string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentException("All fields are required.");
            }

            if (_context.Configurations.Any(c => c.Name == name))
            {
                throw new InvalidOperationException("A configuration with the same name already exists.");
            }

            var config = ConfigurationFactory.CreateConfiguration(name, type, value, applicationName);
            _context.Configurations.Add(config);
            _context.SaveChanges();
        }

        public void UpdateConfiguration(string name, string type, string value, bool isActive, string applicationName)
        {
            var config = _context.Configurations.FirstOrDefault(c => c.Name == name);
            if (config != null)
            {
                config.Type = type;
                config.Value = value;
                config.IsActive = isActive;
                config.ApplicationName = applicationName;
                config.UpdatedDate = DateTime.UtcNow;

                _context.SaveChanges();
            }
        }

        public void DeleteConfiguration(string name)
        {
            var config = _context.Configurations.FirstOrDefault(c => c.Name == name);
            if (config != null)
            {
                _context.Configurations.Remove(config);
                _context.SaveChanges();
            }
        }
    }
}
