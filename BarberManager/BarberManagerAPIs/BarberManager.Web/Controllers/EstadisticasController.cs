using System.Net.Http.Json;
using BarberManager.Web.Models;
using BarberManager.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BarberManager.Web.Controllers;

[RequiereSesion]
public class EstadisticasController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private static readonly string[] OrdenDias = ["lunes", "martes", "miércoles", "jueves", "viernes", "sábado", "domingo"];

    public EstadisticasController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public async Task<IActionResult> Index()
    {
        try
        {
            var estadisticas = await _httpClientFactory.CreateClient("BarberApi")
                .GetFromJsonAsync<List<EstadisticaViewModel>>("estadisticas") ?? [];
            var historial = estadisticas.GroupBy(e => e.Fecha.Date).Select(g => new EstadisticaViewModel { Fecha = g.Key, NombreDia = g.First().NombreDia, CantServicios = g.Sum(e => e.CantServicios), CantVentas = g.Sum(e => e.CantVentas) }).OrderByDescending(e => e.Fecha).ToList();
            var promedios = historial.GroupBy(e => e.NombreDia.ToLower()).Select(g => new PromedioDiaViewModel { Dia = char.ToUpper(g.Key[0]) + g.Key[1..], PromedioServicios = Math.Round((decimal)g.Average(e => e.CantServicios), 1), PromedioVentas = Math.Round((decimal)g.Average(e => e.CantVentas), 1) }).OrderBy(p => Array.IndexOf(OrdenDias, p.Dia.ToLower())).ToList();
            return View(new EstadisticasPaginaViewModel { TotalServicios = historial.Sum(e => e.CantServicios), TotalVentas = historial.Sum(e => e.CantVentas), Historial = historial, Promedios = promedios });
        }
        catch (HttpRequestException)
        {
            ViewBag.ErrorApi = "No se pudo conectar con la API.";
            return View(new EstadisticasPaginaViewModel());
        }
    }
}
