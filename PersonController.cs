using Microsoft.AspNetCore.Mvc;
using PersonApi.Models;
using PersonApi.Services;

namespace PersonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly FileManager _fileManager;

        public PersonController()
        {
            _fileManager = FileManager.Instance;
        }

        // GET api/person
        [HttpGet]
        [ProducesResponseType(typeof(List<Person>), StatusCodes.Status200OK)]
        public ActionResult<List<Person>> GetAll()
        {
            var persons = _fileManager.GetAll();
            return Ok(persons);
        }

        // GET api/person/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Person> GetById(string id)
        {
            var person = _fileManager.GetById(id);
            if (person is null)
                return NotFound(new { message = $"Person with id '{id}' not found." });

            return Ok(person);
        }

        // POST api/person
        [HttpPost]
        [ProducesResponseType(typeof(Person), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<Person> Create([FromBody] Person person)
        {
            if (person is null)
                return BadRequest(new { message = "Request body cannot be null." });

            if (string.IsNullOrWhiteSpace(person.Name))
                return BadRequest(new { message = "Name is required." });

            try
            {
                var created = _fileManager.Add(person);
                return CreatedAtAction(nameof(GetById),
                    new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT api/person/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Person> Update(string id, [FromBody] Person person)
        {
            if (person is null)
                return BadRequest(new { message = "Request body cannot be null." });

            var updated = _fileManager.Update(id, person);
            if (updated is null)
                return NotFound(new { message = $"Person with id '{id}' not found." });

            return Ok(updated);
        }

        // DELETE api/person/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(string id)
        {
            var deleted = _fileManager.Delete(id);
            if (!deleted)
                return NotFound(new { message = $"Person with id '{id}' not found." });

            return NoContent();
        }
    }
}
