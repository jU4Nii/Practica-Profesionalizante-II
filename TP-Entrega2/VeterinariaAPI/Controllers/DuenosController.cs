using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VeterinariaAPI.Data;
using VeterinariaAPI.DTOs;
using VeterinariaAPI.Models;

namespace VeterinariaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DuenosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DuenosController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public List<Dueno> Get()
        {
            return _context.Duenos.ToList();
        }

        
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var dueno = _context.Duenos.FirstOrDefault(d => d.Id == id);

            if (dueno == null)
                return NotFound();

            return Ok(dueno);
        }

        
        [HttpPost]
        public IActionResult Post(DuenoDTO dto)
        {
            Dueno dueno = new Dueno
            {
                Dni = dto.Dni,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido
            };

            _context.Duenos.Add(dueno);
            _context.SaveChanges();

            return StatusCode(201, new { mensaje = "Dueño creado", id = dueno.Id });
        }

       
        [HttpPut("{id}")]
        public IActionResult Put(int id, DuenoDTO dto)
        {
            var dueno = _context.Duenos.FirstOrDefault(d => d.Id == id);

            if (dueno == null)
                return NotFound();

            dueno.Dni = dto.Dni;
            dueno.Nombre = dto.Nombre;
            dueno.Apellido = dto.Apellido;

            _context.SaveChanges();

            return Ok(new { mensaje = "Dueño actualizado correctamente" });
        }

      
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dueno = _context.Duenos.FirstOrDefault(d => d.Id == id);

            if (dueno == null)
                return NotFound();

            _context.Duenos.Remove(dueno);
            _context.SaveChanges();

            return NoContent();
        }
    }
}