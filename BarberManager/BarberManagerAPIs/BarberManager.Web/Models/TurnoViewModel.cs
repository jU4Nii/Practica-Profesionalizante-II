using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BarberManager.Web.Models;

public class TurnoViewModel
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public int IdPeluquero { get; set; }
    public DateTime Fecha { get; set; }
    public string Hora { get; set; } = string.Empty;
    public bool Cancelado { get; set; }
}

public class TurnoListadoViewModel : TurnoViewModel
{
    public string NombreCliente { get; set; } = string.Empty;
    public string NombrePeluquero { get; set; } = string.Empty;
}

public class NuevoTurnoViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná un cliente.")]
    public int IdCliente { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná un peluquero.")]
    public int IdPeluquero { get; set; }

    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Ingresá un horario.")]
    public string Hora { get; set; } = string.Empty;

    public List<SelectListItem> Clientes { get; set; } = [];
    public List<SelectListItem> Peluqueros { get; set; } = [];
}
