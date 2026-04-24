using Microsoft.AspNetCore.Mvc;
using VeterinariaAPI.Models;
using VeterinariaAPI.DTOs;
using System.Linq;

namespace VeterinariaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DuenosController : ControllerBase
    {
        static List<Dueno> duenos = new List<Dueno>();

        [HttpGet]
        public List<Dueno> Get()
        {
            return duenos;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var dueno = duenos.FirstOrDefault(d => d.Id == id);

            if (dueno == null)
                return NotFound();

            return Ok(dueno);
        }

        [HttpPost]
        public IActionResult Post(DuenoDTO dto)
        {
            Dueno dueno = new Dueno
            {
                Id = duenos.Count + 1,
                Dni = dto.Dni,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido
            };

            duenos.Add(dueno);

            return StatusCode(201, new { mensaje = "Dueño creado" });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, DuenoDTO dto)
        {
            var dueno = duenos.FirstOrDefault(d => d.Id == id);

            if (dueno == null)
                return NotFound();

            dueno.Dni = dto.Dni;
            dueno.Nombre = dto.Nombre;
            dueno.Apellido = dto.Apellido;

            return Ok(new { mensaje = "Dueño actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dueno = duenos.FirstOrDefault(d => d.Id == id);

            if (dueno == null)
                return NotFound();

            duenos.Remove(dueno);

            return NoContent();
        }
    }
}
