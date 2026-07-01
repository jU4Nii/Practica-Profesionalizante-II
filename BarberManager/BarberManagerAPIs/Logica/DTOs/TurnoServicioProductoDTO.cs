namespace BarberManagerAPIs.Logica.DTOs
{
    public class TurnoServicioProductoDTO
    {
        public int IdTurno { get; set; }

        public int? IdServicio { get; set; }

        public int? IdProducto { get; set; }

        public int CantidadProducto { get; set; }

        public decimal PrecioUnitario { get; set; }
    }
}