using System.Net.Http.Json;
using BarberManager.Web.Models;
using BarberManager.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BarberManager.Web.Controllers;

[RequiereSesion]
public class ClientesController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ClientesController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        try
        {
            var cliente = _httpClientFactory.CreateClient("BarberApi");
            var clientes = await cliente.GetFromJsonAsync<List<ClienteViewModel>>("clientes") ?? [];
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                clientes = clientes.Where(c =>
                    c.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase) ||
                    c.Telefono.Contains(buscar, StringComparison.OrdinalIgnoreCase) ||
                    (c.Correo?.Contains(buscar, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
            }
            ViewData["Buscar"] = buscar;
            return View(clientes);
        }
        catch (HttpRequestException)
        {
            ViewBag.ErrorApi = "No se pudo conectar con la API. Verificá que esté ejecutándose en http://localhost:5034.";
            return View(new List<ClienteViewModel>());
        }
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClienteViewModel clienteNuevo)
    {
        if (!ModelState.IsValid) return View(clienteNuevo);

        HttpResponseMessage respuesta;
        try
        {
            var cliente = _httpClientFactory.CreateClient("BarberApi");
            respuesta = await cliente.PostAsJsonAsync("clientes", clienteNuevo);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(clienteNuevo);
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudo crear el cliente.");
            return View(clienteNuevo);
        }

        TempData["Mensaje"] = "Cliente creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var cliente = await _httpClientFactory.CreateClient("BarberApi").GetFromJsonAsync<ClienteViewModel>($"clientes/{id}");
            return cliente is null ? NotFound() : View(cliente);
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo conectar con la API.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ClienteViewModel cliente)
    {
        if (!ModelState.IsValid) return View(cliente);

        try
        {
            var respuesta = await _httpClientFactory.CreateClient("BarberApi").PutAsJsonAsync($"clientes/{id}", cliente);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el cliente.");
                return View(cliente);
            }
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(cliente);
        }

        TempData["Mensaje"] = "Cliente actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var cliente = await api.GetFromJsonAsync<ClienteViewModel>($"clientes/{id}");
            if (cliente is null) return NotFound();

            var turnos = await api.GetFromJsonAsync<List<TurnoHistorialApiViewModel>>("turnos") ?? [];
            var peluqueros = await api.GetFromJsonAsync<List<PeluqueroHistorialApiViewModel>>("peluqueros") ?? [];
            var items = await api.GetFromJsonAsync<List<TurnoItemHistorialApiViewModel>>("turnos/items") ?? [];
            var servicios = await api.GetFromJsonAsync<List<ServicioHistorialApiViewModel>>("servicios") ?? [];

            var visitas = turnos.Where(t => t.IdCliente == id)
                .OrderByDescending(t => t.Fecha).ThenByDescending(t => t.Hora)
                .Select(t => new VisitaClienteViewModel
                {
                    Fecha = t.Fecha,
                    Hora = t.Hora,
                    Cancelado = t.Cancelado,
                    Peluquero = peluqueros.FirstOrDefault(p => p.Id == t.IdPeluquero)?.Nombre ?? "Peluquero no encontrado",
                    Servicios = string.Join(", ", items.Where(i => i.IdTurno == t.Id && i.IdServicio.HasValue)
                        .Select(i => servicios.FirstOrDefault(s => s.Id == i.IdServicio)?.Nombre)
                        .Where(nombre => !string.IsNullOrWhiteSpace(nombre)))
                }).ToList();

            foreach (var visita in visitas.Where(v => string.IsNullOrWhiteSpace(v.Servicios)))
                visita.Servicios = "Sin servicios asociados";

            return View(new ClienteHistorialViewModel { Cliente = cliente, Visitas = visitas });
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo conectar con la API.";
            return RedirectToAction(nameof(Index));
        }
    }

    private class TurnoHistorialApiViewModel
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdPeluquero { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; } = string.Empty;
        public bool Cancelado { get; set; }
    }

    private class PeluqueroHistorialApiViewModel { public int Id { get; set; } public string Nombre { get; set; } = string.Empty; }
    private class TurnoItemHistorialApiViewModel { public int IdTurno { get; set; } public int? IdServicio { get; set; } }
    private class ServicioHistorialApiViewModel { public int Id { get; set; } public string Nombre { get; set; } = string.Empty; }
}
