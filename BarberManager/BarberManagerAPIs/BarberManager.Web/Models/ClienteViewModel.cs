using System.ComponentModel.DataAnnotations;

namespace BarberManager.Web.Models;

public class ClienteViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresá el nombre del cliente.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá el teléfono.")]
    public string Telefono { get; set; } = string.Empty;

    public string? Correo { get; set; }

    public string? Notas { get; set; }
}

public class ClienteHistorialViewModel
{
    public ClienteViewModel Cliente { get; set; } = new();
    public List<VisitaClienteViewModel> Visitas { get; set; } = [];
}

public class VisitaClienteViewModel
{
    public DateTime Fecha { get; set; }
    public string Hora { get; set; } = string.Empty;
    public string Peluquero { get; set; } = string.Empty;
    public string Servicios { get; set; } = "Sin servicios asociados";
    public bool Cancelado { get; set; }
}
