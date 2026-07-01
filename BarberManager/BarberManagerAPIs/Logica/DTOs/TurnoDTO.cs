namespace BarberManagerAPIs.Logica.DTOs
{
    public class TurnoDTO
    {
        public int IdCliente { get; set; }

        public int IdPeluquero { get; set; }

        public DateTime Fecha { get; set; }

        public string Hora { get; set; }

        public int? IdPromocion { get; set; }
    }
}