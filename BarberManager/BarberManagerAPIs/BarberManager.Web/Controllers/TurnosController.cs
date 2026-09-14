using System.Net.Http.Json;
using BarberManager.Web.Models;
using BarberManager.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BarberManager.Web.Controllers;

[RequiereSesion]
public class TurnosController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TurnosController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var turnos = await api.GetFromJsonAsync<List<TurnoViewModel>>("turnos") ?? [];
            var clientes = await api.GetFromJsonAsync<List<ClienteViewModel>>("clientes") ?? [];
            var peluqueros = await api.GetFromJsonAsync<List<PeluqueroSimpleViewModel>>("peluqueros") ?? [];

            var listado = turnos
                .Select(t => new TurnoListadoViewModel
                {
                    Id = t.Id,
                    IdCliente = t.IdCliente,
                    IdPeluquero = t.IdPeluquero,
                    Fecha = t.Fecha,
                    Hora = t.Hora,
                    Cancelado = t.Cancelado,
                    NombreCliente = clientes.FirstOrDefault(c => c.Id == t.IdCliente)?.Nombre ?? "Cliente no encontrado",
                    NombrePeluquero = peluqueros.FirstOrDefault(p => p.Id == t.IdPeluquero)?.Nombre ?? "Peluquero no encontrado"
                })
                .OrderByDescending(t => t.Fecha)
                .ThenByDescending(t => t.Hora)
                .ToList();

            return View(listado);
        }
        catch (HttpRequestException)
        {
            ViewBag.ErrorApi = "No se pudo conectar con la API.";
            return View(new List<TurnoListadoViewModel>());
        }
    }

    public async Task<IActionResult> Create()
    {
        var modelo = new NuevoTurnoViewModel();
        await CargarOpciones(modelo);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NuevoTurnoViewModel turno)
    {
        if (!ModelState.IsValid)
        {
            await CargarOpciones(turno);
            return View(turno);
        }

        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var respuesta = await api.PostAsJsonAsync("turnos", turno);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo registrar el turno. El horario puede estar ocupado.");
                await CargarOpciones(turno);
                return View(turno);
            }

            var turnoCreado = await respuesta.Content.ReadFromJsonAsync<TurnoCreadoResponse>();
            if (turnoCreado?.Id is not int idTurno)
            {
                ModelState.AddModelError(string.Empty, "El turno se creó, pero la API no devolvió su identificador.");
                await CargarOpciones(turno);
                return View(turno);
            }

            if (turno.IdProducto.HasValue)
            {
                try
                {
                    var producto = await api.GetFromJsonAsync<ProductoViewModel>($"productos/{turno.IdProducto.Value}");
                    if (producto != null)
                    {
                        var item = new TurnoProductoViewModel
                        {
                            IdTurno = idTurno,
                            IdProducto = producto.Id,
                            CantidadProducto = turno.CantidadProducto,
                            PrecioUnitario = producto.Precio
                        };
                        var respuestaItem = await api.PostAsJsonAsync("turnos/items", item);
                        TempData["Mensaje"] = respuestaItem.IsSuccessStatusCode
                            ? "Turno y producto asociado creados correctamente."
                            : "Turno creado, pero no se pudo asociar el producto.";
                    }
                    else
                    {
                        TempData["Mensaje"] = "Turno creado, pero el producto seleccionado ya no existe.";
                    }
                }
                catch (HttpRequestException)
                {
                    TempData["Mensaje"] = "Turno creado, pero no se pudo asociar el producto.";
                }
            }
            else
            {
                TempData["Mensaje"] = "Turno creado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            await CargarOpciones(turno);
            return View(turno);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var respuesta = await api.DeleteAsync($"turnos/{id}");
            TempData["Mensaje"] = respuesta.IsSuccessStatusCode ? "Turno cancelado." : "No se pudo cancelar el turno.";
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo conectar con la API.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpciones(NuevoTurnoViewModel modelo)
    {
        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var clientes = await api.GetFromJsonAsync<List<ClienteViewModel>>("clientes") ?? [];
            var peluqueros = await api.GetFromJsonAsync<List<PeluqueroSimpleViewModel>>("peluqueros") ?? [];
            var productos = await api.GetFromJsonAsync<List<ProductoViewModel>>("productos") ?? [];

            modelo.Clientes = clientes.Select(c => new SelectListItem(c.Nombre, c.Id.ToString())).ToList();
            modelo.Peluqueros = peluqueros.Where(p => p.EstaActivo).Select(p => new SelectListItem(p.Nombre, p.Id.ToString())).ToList();
            modelo.Productos = productos.OrderBy(p => p.Nombre).Select(p => new SelectListItem(p.Nombre, p.Id.ToString())).ToList();
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron cargar clientes y peluqueros. Verificá que la API esté iniciada.");
        }
    }

    private class PeluqueroSimpleViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool EstaActivo { get; set; }
    }

    private class TurnoCreadoResponse
    {
        public int Id { get; set; }
    }

    private class TurnoProductoViewModel
    {
        public int IdTurno { get; set; }
        public int? IdProducto { get; set; }
        public int CantidadProducto { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
