namespace ConfigurationAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IConfigurationRepository Configurations { get; }
        IApplicationRepository Applications { get; }
        Task<int> CompleteAsync();
    }
}
