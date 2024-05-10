using ConfigurationAPI.Services;
using ConfigurationLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationService _applicationService;

        public ApplicationController(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Application>> GetAllApplications()
        {
            return Ok(_applicationService.GetAllApplications());
        }

        [HttpGet("{serviceName}")]
        public ActionResult<Application> GetApplicationByServiceName(string serviceName)
        {
            var app = _applicationService.GetApplicationByServiceName(serviceName);
            if (app == null)
            {
                return NotFound();
            }
            return Ok(app);
        }

        [HttpPost]
        public ActionResult AddApplication(string serviceName, string applicationUrl)
        {
            try
            {
                _applicationService.AddApplication(serviceName, applicationUrl);
                return CreatedAtAction(nameof(GetApplicationByServiceName), new { serviceName = serviceName }, null);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{serviceName}")]
        public ActionResult UpdateApplication(string serviceName, string applicationUrl)
        {
            var existingApp = _applicationService.GetApplicationByServiceName(serviceName);
            if (existingApp == null)
            {
                return NotFound();
            }

            try
            {
                _applicationService.UpdateApplication(serviceName, applicationUrl);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{serviceName}")]
        public ActionResult DeleteApplication(string serviceName)
        {
            var existingApp = _applicationService.GetApplicationByServiceName(serviceName);
            if (existingApp == null)
            {
                return NotFound();
            }

            _applicationService.DeleteApplication(serviceName);
            return NoContent();
        }
    }
}
