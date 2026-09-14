using System.ComponentModel.DataAnnotations;

namespace BarberManager.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ingresá tu correo.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá tu contraseña.")]
    [DataType(DataType.Password)]
    public string Contrasena { get; set; } = string.Empty;
}

public class UsuarioAutenticadoViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public bool EsAdmin { get; set; }
}
