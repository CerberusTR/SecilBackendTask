using ConfigurationAPI.Repositories;
using ConfigurationLibrary;

namespace ConfigurationAPI.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Application> GetApplicationByServiceNameAsync(string serviceName)
        {
            return await _unitOfWork.Applications.GetApplicationByServiceNameAsync(serviceName);
        }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
        {
            return await _unitOfWork.Applications.GetAllApplicationsAsync();
        }

        public async Task AddApplicationAsync(string serviceName, string applicationUrl)
        {
            if (string.IsNullOrEmpty(serviceName) || string.IsNullOrEmpty(applicationUrl))
            {
                throw new ArgumentException("All fields are required.");
            }

            var existingApp = await _unitOfWork.Applications.GetApplicationByServiceNameAsync(serviceName);
            if (existingApp != null)
            {
                throw new InvalidOperationException("An application with the same service name already exists.");
            }

            var application = new Application
            {
                ServiceName = serviceName,
                ApplicationUrl = applicationUrl
            };

            await _unitOfWork.Applications.AddApplicationAsync(application);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateApplicationAsync(string serviceName, string applicationUrl)
        {
            var application = await _unitOfWork.Applications.GetApplicationByServiceNameAsync(serviceName);
            if (application != null)
            {
                application.ApplicationUrl = applicationUrl;

                await _unitOfWork.Applications.UpdateApplicationAsync(application);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteApplicationAsync(string serviceName)
        {
            await _unitOfWork.Applications.DeleteApplicationAsync(serviceName);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<string> GetDataAsync(string serviceName)
        {
            // Implement your data fetching logic here
            // This is just an example, replace it with actual logic
            return await Task.FromResult($"Fetched data for {serviceName}");
        }
    }

}
