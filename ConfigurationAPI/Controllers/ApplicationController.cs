using ConfigurationAPI.Models;
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
        public async Task<ActionResult<Response<IEnumerable<object>>>> GetAllApplications()
        {
            var applications = await _applicationService.GetAllApplicationsAsync();

            var result = applications.Select(app => new
            {
                app.Id,
                app.ServiceName,
                app.ApplicationUrl
            });

            return Ok(new Response<IEnumerable<object>>
            {
                StatusCode = 200,
                Message = "Applications retrieved successfully.",
                Data = result
            });
        }

        [HttpGet("{serviceName}")]
        public async Task<ActionResult<Response<object>>> GetApplicationByServiceName(string serviceName)
        {
            var application = await _applicationService.GetApplicationByServiceNameAsync(serviceName);
            if (application == null)
            {
                return NotFound(new Response<object>
                {
                    StatusCode = 404,
                    Message = "Application not found.",
                    Data = null
                });
            }

            var result = new
            {
                application.Id,
                application.ServiceName,
                application.ApplicationUrl
            };

            return Ok(new Response<object>
            {
                StatusCode = 200,
                Message = "Application retrieved successfully.",
                Data = result
            });
        }

        [HttpPost]
        public async Task<ActionResult<Response<object>>> AddApplication(string serviceName, string applicationUrl)
        {
            try
            {
                await _applicationService.AddApplicationAsync(serviceName, applicationUrl);
                return CreatedAtAction(nameof(GetApplicationByServiceName), new { serviceName = serviceName }, new Response<object>
                {
                    StatusCode = 201,
                    Message = "Application added successfully.",
                    Data = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new Response<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new Response<object>
                {
                    StatusCode = 409,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPut("{serviceName}")]
        public async Task<ActionResult<Response<object>>> UpdateApplication(string serviceName, string applicationUrl)
        {
            var existingApp = await _applicationService.GetApplicationByServiceNameAsync(serviceName);
            if (existingApp == null)
            {
                return NotFound(new Response<object>
                {
                    StatusCode = 404,
                    Message = "Application not found.",
                    Data = null
                });
            }

            await _applicationService.UpdateApplicationAsync(serviceName, applicationUrl);
            return Ok(new Response<object>
            {
                StatusCode = 200,
                Message = "Application updated successfully.",
                Data = null
            });
        }

        [HttpDelete("{serviceName}")]
        public async Task<ActionResult<Response<object>>> DeleteApplication(string serviceName)
        {
            var existingApp = await _applicationService.GetApplicationByServiceNameAsync(serviceName);
            if (existingApp == null)
            {
                return NotFound(new Response<object>
                {
                    StatusCode = 404,
                    Message = "Application not found.",
                    Data = null
                });
            }

            await _applicationService.DeleteApplicationAsync(serviceName);
            return Ok(new Response<object>
            {
                StatusCode = 200,
                Message = "Application deleted successfully.",
                Data = null
            });
        }
    }
}
