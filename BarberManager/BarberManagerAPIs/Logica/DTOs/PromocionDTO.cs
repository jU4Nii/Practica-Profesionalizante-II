namespace BarberManagerAPIs.Logica.DTOs
{
    public class PromocionDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DescuentoPorcentaje { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}