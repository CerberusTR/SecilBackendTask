
using ConfigurationAPI.Helpers.ConfigurationAPI.Helpers;
using ConfigurationAPI.Models;
using ConfigurationAPI.Services;
using ConfigurationLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationService _configurationService;

        public ConfigurationController(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<object>>>> GetAllConfigurations()
        {
            var configurations = await _configurationService.GetAllConfigurationsAsync();

            var result = configurations.Select(config => new
            {
                config.Id,
                config.Name,
                config.Type,
                config.Value,
                config.IsActive,
                config.ApplicationName,
                config.UpdatedDate
            });

            return Ok(new Response<IEnumerable<object>>
            {
                StatusCode = 200,
                Message = "Configurations retrieved successfully.",
                Data = result
            });
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Response<object>>> GetConfigurationByName(string name)
        {
            var config = await _configurationService.GetConfigurationByNameAsync(name);
            if (config == null)
            {
                return NotFound(new Response<object>
                {
                    StatusCode = 404,
                    Message = "Configuration not found.",
                    Data = null
                });
            }

            var response = ResponseHelper.CreateResponse(config.Type, 200, "Configuration retrieved successfully.", config.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<object>>> AddConfiguration(string name, string type, string value, string applicationName)
        {
            try
            {
                await _configurationService.AddConfigurationAsync(name, type, value, applicationName);
                return CreatedAtAction(nameof(GetConfigurationByName), new { name = name },
                    ResponseHelper.CreateResponse(type, 201, "Configuration added successfully.", value));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseHelper.CreateResponse(type, 400, ex.Message, null));
            }
        }

        [HttpPut("{name}")]
        public async Task<ActionResult<Response<object>>> UpdateConfiguration(string name, string type, string value, bool isActive, string applicationName)
        {
            var existingConfig = await _configurationService.GetConfigurationByNameAsync(name);
            if (existingConfig == null)
            {
                return NotFound(ResponseHelper.CreateResponse(type, 404, "Configuration not found.", null));
            }

            await _configurationService.UpdateConfigurationAsync(name, type, value, isActive, applicationName);
            return Ok(ResponseHelper.CreateResponse(type, 200, "Configuration updated successfully.", value));
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult<Response<object>>> DeleteConfiguration(string name)
        {
            var existingConfig = await _configurationService.GetConfigurationByNameAsync(name);
            if (existingConfig == null)
            {
                return NotFound(new Response<object>
                {
                    StatusCode = 404,
                    Message = "Configuration not found.",
                    Data = null
                });
            }

            await _configurationService.DeleteConfigurationAsync(name);
            return Ok(new Response<object>
            {
                StatusCode = 200,
                Message = "Configuration deleted successfully.",
                Data = null
            });
        }
    }
}
