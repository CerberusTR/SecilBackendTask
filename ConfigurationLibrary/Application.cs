using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary
{
    public class Application
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ServiceName { get; set; }
        public string ApplicationUrl { get; set; }
    }
}
