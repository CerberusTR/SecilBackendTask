using ConfigurationLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationReader _configurationReader;

        public ConfigurationController(ConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader;
        }

        [HttpGet("{key}")]
        public IActionResult GetConfigurationValue(string key)
        {
            try
            {
                // key parameter corresponds to the Name property in the Configuration model
                var value = _configurationReader.GetValue<string>(key);
                return Ok(value);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
