using ConfigurationLibrary;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationAPI.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ConfigurationContext _context;

        public ApplicationRepository(ConfigurationContext context)
        {
            _context = context;
        }

        public async Task<Application> GetApplicationByServiceNameAsync(string serviceName)
        {
            return await _context.Applications.FirstOrDefaultAsync(app => app.ServiceName == serviceName);
        }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
        {
            return await _context.Applications.ToListAsync();
        }

        public async Task AddApplicationAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
        }

        public async Task UpdateApplicationAsync(Application application)
        {
            _context.Applications.Update(application);
        }

        public async Task DeleteApplicationAsync(string serviceName)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(app => app.ServiceName == serviceName);
            if (application != null)
            {
                _context.Applications.Remove(application);
            }
        }
    }
}
