namespace BarberManagerAPIs.Entidades
{
    public class Caja
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public string Concepto { get; set; }

        public string MetodoPago { get; set; }

        public bool EsIngreso { get; set; }
    }
}