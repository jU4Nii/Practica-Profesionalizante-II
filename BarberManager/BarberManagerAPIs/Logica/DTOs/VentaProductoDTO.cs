namespace BarberManagerAPIs.Logica.DTOs;

public class VentaProductoDTO
{
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public DateTime Fecha { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
}
