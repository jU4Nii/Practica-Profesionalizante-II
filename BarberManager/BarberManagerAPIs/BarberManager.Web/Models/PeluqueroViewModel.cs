using System.ComponentModel.DataAnnotations;

namespace BarberManager.Web.Models;

public class PeluqueroViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresá el nombre.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá el correo.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá el teléfono.")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá una contraseña.")]
    [DataType(DataType.Password)]
    public string Contrasena { get; set; } = string.Empty;

    public bool EsAdmin { get; set; }
    public bool EstaActivo { get; set; } = true;
}
