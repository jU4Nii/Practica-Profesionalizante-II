namespace BarberManagerAPIs.Logica.DTOs
{
    public class CajaDTO
    {
        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public string Concepto { get; set; }

        public string MetodoPago { get; set; }

        public bool EsIngreso { get; set; }
    }
}