using System.ComponentModel.DataAnnotations;

namespace BarberManager.Web.Models;

public class ClienteViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresá el nombre del cliente.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá el teléfono.")]
    public string Telefono { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Ingresá un correo válido.")]
    public string? Correo { get; set; }

    public string? Notas { get; set; }
}
