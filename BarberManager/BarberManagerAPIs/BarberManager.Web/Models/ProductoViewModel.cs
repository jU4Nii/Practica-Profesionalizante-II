using System.ComponentModel.DataAnnotations;

namespace BarberManager.Web.Models;

public class ProductoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresá el nombre del producto.")]
    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "La cantidad no puede ser negativa.")]
    public int Cantidad { get; set; }

    public bool UsoInterno { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
    public decimal Precio { get; set; }
}
