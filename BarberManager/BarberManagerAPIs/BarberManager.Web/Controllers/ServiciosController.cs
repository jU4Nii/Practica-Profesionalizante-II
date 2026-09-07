using System.Net.Http.Json;
using BarberManager.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarberManager.Web.Controllers;

public class ServiciosController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ServiciosController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var cliente = _httpClientFactory.CreateClient("BarberApi");
            var servicios = await cliente.GetFromJsonAsync<List<ServicioViewModel>>("servicios") ?? [];
            return View(servicios);
        }
        catch (HttpRequestException)
        {
            ViewBag.ErrorApi = "No se pudo conectar con la API. Verificá que esté ejecutándose en http://localhost:5034.";
            return View(new List<ServicioViewModel>());
        }
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServicioViewModel servicio)
    {
        if (!ModelState.IsValid) return View(servicio);

        HttpResponseMessage respuesta;
        try
        {
            var cliente = _httpClientFactory.CreateClient("BarberApi");
            respuesta = await cliente.PostAsJsonAsync("servicios", servicio);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(servicio);
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudo crear el servicio.");
            return View(servicio);
        }

        TempData["Mensaje"] = "Servicio creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var cliente = _httpClientFactory.CreateClient("BarberApi");
            var servicio = await cliente.GetFromJsonAsync<ServicioViewModel>($"servicios/{id}");
            return servicio is null ? NotFound() : View(servicio);
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo conectar con la API.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ServicioViewModel servicio)
    {
        if (!ModelState.IsValid) return View(servicio);

        HttpResponseMessage respuesta;
        try
        {
            var cliente = _httpClientFactory.CreateClient("BarberApi");
            respuesta = await cliente.PutAsJsonAsync($"servicios/{id}", servicio);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(servicio);
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudo actualizar el servicio.");
            return View(servicio);
        }

        TempData["Mensaje"] = "Servicio actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var cliente = _httpClientFactory.CreateClient("BarberApi");
            var respuesta = await cliente.DeleteAsync($"servicios/{id}");
            TempData["Mensaje"] = respuesta.IsSuccessStatusCode
                ? "Servicio eliminado correctamente."
                : "No se pudo eliminar el servicio.";
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo conectar con la API.";
        }
        return RedirectToAction(nameof(Index));
    }
}
