using System.ComponentModel.DataAnnotations;

namespace BarberManager.Web.Models;

public class ServicioViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresá el nombre del servicio.")]
    public string Nombre { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
    public decimal Precio { get; set; }
}
