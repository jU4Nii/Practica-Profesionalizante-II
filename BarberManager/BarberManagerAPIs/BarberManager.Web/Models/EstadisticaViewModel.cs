namespace BarberManager.Web.Models;

public class EstadisticaViewModel
{
    public int Id { get; set; }
    public string NombreDia { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int CantServicios { get; set; }
    public int CantVentas { get; set; }
}

public class EstadisticasPaginaViewModel
{
    public int TotalServicios { get; set; }
    public int TotalVentas { get; set; }
    public List<EstadisticaViewModel> Historial { get; set; } = [];
    public List<PromedioDiaViewModel> Promedios { get; set; } = [];
}

public class PromedioDiaViewModel
{
    public string Dia { get; set; } = string.Empty;
    public decimal PromedioServicios { get; set; }
    public decimal PromedioVentas { get; set; }
}
