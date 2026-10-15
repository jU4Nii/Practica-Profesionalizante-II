namespace BarberManagerAPIs.Logica.DTOs;

public class VentaProductoDTO
{
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public int CantidadProducto
    {
        get => Cantidad;
        set { if (value > 0) Cantidad = value; }
    }
    public DateTime Fecha { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
}
