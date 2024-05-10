using ConfigurationLibrary;

namespace ConfigurationAPI.Services
{
    public class ApplicationService
    {
        private readonly ConfigurationContext _context;

        public ApplicationService(ConfigurationContext context)
        {
            _context = context;
        }

        public Application GetApplicationByServiceName(string serviceName)
        {
            return _context.Applications.FirstOrDefault(app => app.ServiceName == serviceName);
        }

        public IEnumerable<Application> GetAllApplications()
        {
            return _context.Applications.ToList();
        }

        public void AddApplication(string serviceName, string applicationUrl)
        {
            if (string.IsNullOrEmpty(serviceName) || string.IsNullOrEmpty(applicationUrl))
            {
                throw new ArgumentException("All fields are required.");
            }

            if (_context.Applications.Any(a => a.ServiceName == serviceName))
            {
                throw new InvalidOperationException("An application with the same service name already exists.");
            }

            var app = new Application
            {
                ServiceName = serviceName,
                ApplicationUrl = applicationUrl
            };
            _context.Applications.Add(app);
            _context.SaveChanges();
        }

        public void UpdateApplication(string serviceName, string applicationUrl)
        {
            var app = _context.Applications.FirstOrDefault(a => a.ServiceName == serviceName);
            if (app != null)
            {
                if (_context.Applications.Any(a => a.ServiceName == serviceName && a.Id != app.Id))
                {
                    throw new InvalidOperationException("An application with the same service name already exists.");
                }

                app.ServiceName = serviceName;
                app.ApplicationUrl = applicationUrl;

                _context.SaveChanges();
            }
        }

        public void DeleteApplication(string serviceName)
        {
            var app = _context.Applications.FirstOrDefault(a => a.ServiceName == serviceName);
            if (app != null)
            {
                _context.Applications.Remove(app);
                _context.SaveChanges();
            }
        }
    }
}
