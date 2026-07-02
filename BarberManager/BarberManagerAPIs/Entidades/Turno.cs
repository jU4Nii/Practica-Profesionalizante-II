namespace BarberManagerAPIs.Entidades
{
    public class Turno
    {
        public int Id { get; set; }

        public int IdCliente { get; set; }

        public int IdPeluquero { get; set; }

        public DateTime Fecha { get; set; }

        public string Hora { get; set; }

        public int? IdPromocion { get; set; }

        public bool Cancelado { get; set; } = false;
    }
}