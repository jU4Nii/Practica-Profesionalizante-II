namespace BarberManagerAPIs.Logica.DTOs
{
    public class EstadisticaDTO
    {
        public string NombreDia { get; set; }
        public DateTime Fecha { get; set; }
        public int CantServicios { get; set; }
        public int CantVentas { get; set; }
    }
}