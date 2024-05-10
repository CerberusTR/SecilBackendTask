using Newtonsoft.Json;
using Service_B.Models;

namespace Service_B.Repositories
{
    public class FileConfigurationRepository
    {
        private readonly string _filePath;

        public FileConfigurationRepository(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<Configuration>()));
            }
        }

        public IEnumerable<Configuration> GetAll()
        {
            var data = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Configuration>>(data);
        }

        public Configuration GetById(Guid id)
        {
            return GetAll().FirstOrDefault(c => c.Id == id);
        }

        public Configuration GetByName(string name)
        {
            return GetAll().FirstOrDefault(c => c.Name == name);
        }

        public void Add(Configuration configuration)
        {
            if (GetByName(configuration.Name) != null)
            {
                throw new ArgumentException("A configuration with the same name already exists.");
            }

            var configurations = GetAll().ToList();
            configurations.Add(configuration);
            SaveAll(configurations);
        }

        public void Update(Configuration configuration)
        {
            var configurations = GetAll().ToList();
            var index = configurations.FindIndex(c => c.Id == configuration.Id);
            if (index != -1)
            {
                configurations[index] = configuration;
                SaveAll(configurations);
            }
        }

        public void Delete(Guid id)
        {
            var configurations = GetAll().ToList();
            var configuration = configurations.FirstOrDefault(c => c.Id == id);
            if (configuration != null)
            {
                configurations.Remove(configuration);
                SaveAll(configurations);
            }
        }

        private void SaveAll(IEnumerable<Configuration> configurations)
        {
            var data = JsonConvert.SerializeObject(configurations, Formatting.Indented);
            File.WriteAllText(_filePath, data);
        }
    }
}
