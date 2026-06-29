namespace BarberManagerAPIs.Logica.DTOs
{
    public class PeluqueroDTO
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Contrasena { get; set; }
        public bool EsAdmin { get; set; }
        public bool EstaActivo { get; set; }
    }
}