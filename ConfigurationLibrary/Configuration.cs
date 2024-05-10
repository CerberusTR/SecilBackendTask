using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary
{
    public class Configuration
    {
        public Guid Id { get; set; }=Guid.NewGuid();
        public string Name { get; set; } //Unique
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; } = false;
        public string ApplicationName { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
