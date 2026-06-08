namespace BarberManagerAPIs.Logica.DTOs
{
    public class ProductoDTO
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Cantidad { get; set; }

        public bool UsoInterno { get; set; }

        public decimal Precio { get; set; }
    }
}