using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using VeterinariaAPI.Data;
using VeterinariaAPI.DTOs;
using VeterinariaAPI.Models;

namespace VeterinariaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AtencionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AtencionesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Atencion> Get()
        {
            return _context.Atenciones.OrderByDescending(a => a.Fecha).ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var atencion = _context.Atenciones.FirstOrDefault(a => a.Id == id);

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
                AnimalId = dto.AnimalId,
                Motivo = dto.Motivo,
                TratamientoId = dto.TratamientoId,
                MedicamentosIds = dto.MedicamentosIds,
                Fecha = dto.Fecha
            };

            _context.Atenciones.Add(atencion);
            _context.SaveChanges();

            return StatusCode(201, new { mensaje = "Atención creada", id = atencion.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AtencionDTO dto)
        {
            var atencion = _context.Atenciones.FirstOrDefault(a => a.Id == id);

            if (atencion == null)
                return NotFound();

            
            if (dto.Fecha > DateTime.Now || dto.Fecha < DateTime.Now.AddDays(-30))
                return BadRequest(new { mensaje = "Fecha inválida" });

            atencion.AnimalId = dto.AnimalId;
            atencion.Motivo = dto.Motivo;
            atencion.TratamientoId = dto.TratamientoId;
            atencion.MedicamentosIds = dto.MedicamentosIds;
            atencion.Fecha = dto.Fecha;

            _context.SaveChanges();

            return Ok(new { mensaje = "Atención actualizada correctamente" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var atencion = _context.Atenciones.FirstOrDefault(a => a.Id == id);

            if (atencion == null)
                return NotFound();

            _context.Atenciones.Remove(atencion);
            _context.SaveChanges();

            return NoContent();
        }
    }
}