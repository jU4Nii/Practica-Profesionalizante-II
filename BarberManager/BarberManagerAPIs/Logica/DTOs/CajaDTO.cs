namespace BarberManagerAPIs.Logica.DTOs
{
    public class CajaDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaCierre { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
    }
}