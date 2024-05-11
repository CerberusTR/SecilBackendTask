using ConfigurationLibrary;

namespace ConfigurationAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ConfigurationContext _context;
        private IConfigurationRepository _configurations;
        private IApplicationRepository _applications;

        public UnitOfWork(ConfigurationContext context)
        {
            _context = context;
        }

        public IConfigurationRepository Configurations => _configurations ??= new ConfigurationRepository(_context);
        public IApplicationRepository Applications => _applications ??= new ApplicationRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
