
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VeterinariaAPI.Data;
using VeterinariaAPI.DTOs;
using VeterinariaAPI.Models;


namespace VeterinariaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnimalesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Animal> Get()
        {
            return _context.Animales.ToList();
        }

        [HttpPost]
        public IActionResult Post(AnimalDTO dto)
        {
            Animal animal = new Animal
            {
                Nombre = dto.Nombre,
                Edad = dto.Edad,
                Sexo = dto.Sexo,
                RazaId = dto.RazaId,
                DuenoId = dto.DuenoId
            };

            _context.Animales.Add(animal);
            _context.SaveChanges();

            return StatusCode(201, new { mensaje = "Animal creado" });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var animal = _context.Animales.FirstOrDefault(a => a.Id == id);

            if (animal == null)
                return NotFound();

            return Ok(animal);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AnimalDTO dto)
        {
            var animal = _context.Animales.FirstOrDefault(a => a.Id == id);

            if (animal == null)
                return NotFound();

            animal.Nombre = dto.Nombre;
            animal.Edad = dto.Edad;
            animal.Sexo = dto.Sexo;
            animal.RazaId = dto.RazaId;
            animal.DuenoId = dto.DuenoId;

            _context.SaveChanges();

            return Ok(new { mensaje = "Animal actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var animal = _context.Animales.FirstOrDefault(a => a.Id == id);

            if (animal == null)
                return NotFound();

            _context.Animales.Remove(animal);
            _context.SaveChanges();

            return NoContent();
        }

    }
}