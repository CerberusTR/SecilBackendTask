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

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var configuration = _repository.GetById(id);
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
                configuration.Id = Guid.NewGuid();
                _repository.Add(configuration);
                return CreatedAtAction(nameof(GetById), new { id = configuration.Id }, configuration);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, Configuration configuration)
        {
            var existingConfiguration = _repository.GetById(id);
            if (existingConfiguration == null)
            {
                return NotFound();
            }

            configuration.Id = id;
            _repository.Update(configuration);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var configuration = _repository.GetById(id);
            if (configuration == null)
            {
                return NotFound();
            }

            _repository.Delete(id);
            return NoContent();
        }
    }
}
