using ConfigurationLibrary;

namespace ConfigurationAPI.Services
{
    public interface IApplicationService
    {
        Task<Application> GetApplicationByServiceNameAsync(string serviceName);
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task AddApplicationAsync(string serviceName, string applicationUrl);
        Task UpdateApplicationAsync(string serviceName, string applicationUrl);
        Task DeleteApplicationAsync(string serviceName);
    }
}
