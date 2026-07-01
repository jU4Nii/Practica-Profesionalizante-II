namespace BarberManagerAPIs.Entidades
{
    public class Promocion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DescuentoPorcentaje { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public List<int> ServiciosIds { get; set; } = new();
        public List<int> ProductosIds { get; set; } = new();
    }
}