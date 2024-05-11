using ConfigurationAPI.Repositories;
using ConfigurationLibrary;
using Microsoft.EntityFrameworkCore;

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
            var existingApp = await _unitOfWork.Applications.GetApplicationByServiceNameAsync(serviceName);
            if (existingApp != null)
            {
                throw new InvalidOperationException("An application with the same service name already exists.");
            }

            var app = new Application
            {
                ServiceName = serviceName,
                ApplicationUrl = applicationUrl
            };

            await _unitOfWork.Applications.AddApplicationAsync(app);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateApplicationAsync(string serviceName, string applicationUrl)
        {
            var app = await _unitOfWork.Applications.GetApplicationByServiceNameAsync(serviceName);
            if (app != null)
            {
                app.ApplicationUrl = applicationUrl;
                await _unitOfWork.Applications.UpdateApplicationAsync(app);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteApplicationAsync(string serviceName)
        {
            await _unitOfWork.Applications.DeleteApplicationAsync(serviceName);
            await _unitOfWork.CompleteAsync();
        }
    }

}
