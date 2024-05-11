namespace ConfigurationAPI.Repositories
{
    public interface IUnitOfWork
    {
        IConfigurationRepository Configurations { get; }
        IApplicationRepository Applications { get; }
        Task<int> CompleteAsync();
    }
}
