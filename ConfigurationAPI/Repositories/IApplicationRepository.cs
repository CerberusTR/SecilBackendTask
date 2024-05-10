using ConfigurationLibrary;

namespace ConfigurationAPI.Repositories
{
    public interface IApplicationRepository
    {
        Task<Application> GetApplicationByServiceNameAsync(string serviceName);
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task AddApplicationAsync(Application application);
        Task UpdateApplicationAsync(Application application);
        Task DeleteApplicationAsync(string serviceName);
    }
}
