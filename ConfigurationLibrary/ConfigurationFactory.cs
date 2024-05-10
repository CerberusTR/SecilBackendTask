using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary
{
    public class ConfigurationFactory
    {
        public static Configuration CreateConfiguration(string name, string type, string value, string applicationName)
        {
            return new Configuration
            {
                Name = name,
                Type = type,
                Value = value,
                ApplicationName = applicationName
            };
        }
    }
}
