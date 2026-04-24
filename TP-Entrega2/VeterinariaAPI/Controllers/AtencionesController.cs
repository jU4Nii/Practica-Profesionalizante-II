using Microsoft.AspNetCore.Mvc;
using VeterinariaAPI.Models;
using VeterinariaAPI.DTOs;
using System.Linq;
using System;

namespace VeterinariaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AtencionesController : ControllerBase
    {
        static List<Atencion> atenciones = new List<Atencion>();

        [HttpGet]
        public List<Atencion> Get()
        {
            return atenciones;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var atencion = atenciones.FirstOrDefault(a => a.Id == id);

            if (atencion == null)
                return NotFound();

            return Ok(atencion);
        }

        [HttpPost]
        public IActionResult Post(AtencionDTO dto)
        {
           
            if (dto.Fecha > DateTime.Now || dto.Fecha < DateTime.Now.AddDays(-30))
                return BadRequest(new { mensaje = "Fecha inválida" });

            Atencion atencion = new Atencion
            {
                Id = atenciones.Count + 1,
                AnimalId = dto.AnimalId,
                Motivo = dto.Motivo,
                TratamientoId = dto.TratamientoId,
                MedicamentosIds = dto.MedicamentosIds,
                Fecha = dto.Fecha
            };

            atenciones.Add(atencion);

            return StatusCode(201, new { mensaje = "Atención creada" });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AtencionDTO dto)
        {
            var atencion = atenciones.FirstOrDefault(a => a.Id == id);

            if (atencion == null)
                return NotFound();

          
            if (dto.Fecha > DateTime.Now || dto.Fecha < DateTime.Now.AddDays(-30))
                return BadRequest(new { mensaje = "Fecha inválida" });

            atencion.AnimalId = dto.AnimalId;
            atencion.Motivo = dto.Motivo;
            atencion.TratamientoId = dto.TratamientoId;
            atencion.MedicamentosIds = dto.MedicamentosIds;
            atencion.Fecha = dto.Fecha;

            return Ok(new { mensaje = "Atención actualizada correctamente" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var atencion = atenciones.FirstOrDefault(a => a.Id == id);

            if (atencion == null)
                return NotFound();

            atenciones.Remove(atencion);

            return NoContent();
        }
    }
}