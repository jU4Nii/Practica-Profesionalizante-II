namespace BarberManagerAPIs.Entidades
{
    public class Peluquero
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Contrasena { get; set; }
        public bool EsAdmin { get; set; }
        public bool EstaActivo { get; set; }
    }
}