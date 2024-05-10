using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly string _connectionString;

        public ConfigRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Configuration> GetActiveConfigurations(string applicationName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Configuration>(
                    "SELECT * FROM Configurations WHERE IsActive = 1 AND ApplicationName = @ApplicationName",
                    new { ApplicationName = applicationName });
            }
        }
    }
}
