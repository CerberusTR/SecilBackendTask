using ConfigurationLibrary;

namespace ConfigurationAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ConfigurationContext _context;

        public UnitOfWork(ConfigurationContext context)
        {
            _context = context;
            Configurations = new ConfigurationRepository(_context);
            Applications = new ApplicationRepository(_context);
        }

        public IConfigurationRepository Configurations { get; private set; }
        public IApplicationRepository Applications { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
