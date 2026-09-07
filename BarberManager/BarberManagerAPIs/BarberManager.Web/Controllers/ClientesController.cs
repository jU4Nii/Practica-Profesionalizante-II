using System.Net.Http.Json;
using BarberManager.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarberManager.Web.Controllers;

public class ClientesController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ClientesController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var cliente = _httpClientFactory.CreateClient("BarberApi");
            var clientes = await cliente.GetFromJsonAsync<List<ClienteViewModel>>("clientes") ?? [];
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
}
