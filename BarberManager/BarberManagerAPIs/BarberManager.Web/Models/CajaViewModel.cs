using System.ComponentModel.DataAnnotations;

namespace BarberManager.Web.Models;

public class CajaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresá la fecha.")]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
    public decimal Monto { get; set; }

    [Required(ErrorMessage = "Ingresá un concepto.")]
    public string Concepto { get; set; } = string.Empty;

    [Required(ErrorMessage = "Seleccioná un método de pago.")]
    public string MetodoPago { get; set; } = string.Empty;

    public bool EsIngreso { get; set; } = true;

    public string TipoIngreso { get; set; } = "Venta de producto";
    public int? IdProducto { get; set; }
    public int CantidadProducto { get; set; } = 1;
    public List<ProductoDisponibleVentaViewModel> ProductosDisponibles { get; set; } = [];
}

public class ProductoDisponibleVentaViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
}
