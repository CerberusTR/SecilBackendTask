
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
        public ActionResult<IEnumerable<Configuration>> GetAllConfigurations()
        {
            var configurations = _configurationService.GetAllConfigurations();

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

            return Ok(result);
        }

        [HttpGet("{name}")]
        public ActionResult<Configuration> GetConfigurationByName(string name)
        {
            var config = _configurationService.GetConfigurationByName(name);
            if (config == null)
            {
                return NotFound();
            }

            var result = new
            {
                config.Id,
                config.Name,
                config.Type,
                config.Value,
                config.IsActive,
                config.ApplicationName,
                config.UpdatedDate
            };

            return Ok(result);
        }

        [HttpPost]
        public ActionResult AddConfiguration(string name, string type, string value, string applicationName)
        {
            try
            {
                _configurationService.AddConfiguration(name, type, value, applicationName);
                return CreatedAtAction(nameof(GetConfigurationByName), new { name = name }, null);
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

        [HttpPut("{name}")]
        public ActionResult UpdateConfiguration(string name, string type, string value, bool isActive, string applicationName)
        {
            var existingConfig = _configurationService.GetConfigurationByName(name);
            if (existingConfig == null)
            {
                return NotFound();
            }

            _configurationService.UpdateConfiguration(name, type, value, isActive, applicationName);
            return NoContent();
        }

        [HttpDelete("{name}")]
        public ActionResult DeleteConfiguration(string name)
        {
            var existingConfig = _configurationService.GetConfigurationByName(name);
            if (existingConfig == null)
            {
                return NotFound();
            }

            _configurationService.DeleteConfiguration(name);
            return NoContent();
        }
    }
}
