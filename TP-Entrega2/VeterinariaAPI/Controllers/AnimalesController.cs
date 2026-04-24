using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeterinariaAPI.DTOs;
using VeterinariaAPI.Models;
using System.Linq;

namespace VeterinariaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalesController : ControllerBase
    {
        static List<Animal> animales = new List<Animal>();

        [HttpGet]
        public List<Animal> Get()
        {
            return animales;
        }

        [HttpPost]
        public IActionResult Post(AnimalDTO dto)
        {
            Animal animal = new Animal
            {
                Id = animales.Count + 1,
                Nombre = dto.Nombre,
                Edad = dto.Edad,
                Sexo = dto.Sexo,
                RazaId = dto.RazaId,
                DuenoId = dto.DuenoId
            };

            animales.Add(animal);

            return StatusCode(201, new { mensaje = "Animal creado" });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var animal = animales.FirstOrDefault(a => a.Id == id);

            if (animal == null)
                return NotFound();

            return Ok(animal);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AnimalDTO dto)
        {
            var animal = animales.FirstOrDefault(a => a.Id == id);

            if (animal == null)
                return NotFound();

            animal.Nombre = dto.Nombre;
            animal.Edad = dto.Edad;
            animal.Sexo = dto.Sexo;
            animal.RazaId = dto.RazaId;
            animal.DuenoId = dto.DuenoId;

            return Ok(new { mensaje = "Animal actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var animal = animales.FirstOrDefault(a => a.Id == id);

            if (animal == null)
                return NotFound();

            animales.Remove(animal);

            return NoContent();
        }

    }
}