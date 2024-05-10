using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_A.Models;
using Service_A.Repositories;

namespace Service_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationsController : ControllerBase
    {
        private readonly FileConfigurationRepository _repository;

        public ConfigurationsController(FileConfigurationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var configurations = _repository.GetAll();
            return Ok(configurations);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var configuration = _repository.GetByName(name);
            if (configuration == null)
            {
                return NotFound();
            }
            return Ok(configuration);
        }

        [HttpPost]
        public IActionResult Create(Configuration configuration)
        {
            try
            {
                _repository.Add(configuration);
                return CreatedAtAction(nameof(GetByName), new { name = configuration.Name }, configuration);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{name}")]
        public IActionResult Update(string name, Configuration configuration)
        {
            var existingConfiguration = _repository.GetByName(name);
            if (existingConfiguration == null)
            {
                return NotFound();
            }

            configuration.Name = name;
            _repository.Update(configuration);
            return NoContent();
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            var configuration = _repository.GetByName(name);
            if (configuration == null)
            {
                return NotFound();
            }

            _repository.Delete(name);
            return NoContent();
        }
    }
}
