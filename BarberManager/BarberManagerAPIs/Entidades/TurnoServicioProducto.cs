namespace BarberManagerAPIs.Entidades
{
    public class TurnoServicioProducto
    {
        public int Id { get; set; }

        public int IdTurno { get; set; }

        public int? IdServicio { get; set; }

        public int? IdProducto { get; set; }

        public int CantidadProducto { get; set; }

        public decimal PrecioUnitario { get; set; }
    }
}