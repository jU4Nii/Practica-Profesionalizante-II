using System.Net.Http.Json;
using BarberManager.Web.Models;
using BarberManager.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BarberManager.Web.Controllers;

[SoloAdmin]
public class PeluquerosController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PeluquerosController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public async Task<IActionResult> Index()
    {
        try
        {
            var peluqueros = await _httpClientFactory.CreateClient("BarberApi")
                .GetFromJsonAsync<List<PeluqueroViewModel>>("peluqueros") ?? [];
            return View(peluqueros.OrderBy(p => p.Nombre).ToList());
        }
        catch (HttpRequestException)
        {
            ViewBag.ErrorApi = "No se pudo conectar con la API.";
            return View(new List<PeluqueroViewModel>());
        }
    }

    public IActionResult Create() => View(new PeluqueroViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PeluqueroViewModel peluquero)
    {
        if (!ModelState.IsValid) return View(peluquero);
        try
        {
            var respuesta = await _httpClientFactory.CreateClient("BarberApi").PostAsJsonAsync("peluqueros", peluquero);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo crear el peluquero.");
                return View(peluquero);
            }
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(peluquero);
        }

        TempData["Mensaje"] = "Peluquero creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var peluquero = await _httpClientFactory.CreateClient("BarberApi").GetFromJsonAsync<PeluqueroViewModel>($"peluqueros/{id}");
            return peluquero is null ? NotFound() : View(peluquero);
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo conectar con la API.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PeluqueroViewModel peluquero)
    {
        if (!ModelState.IsValid) return View(peluquero);
        try
        {
            var respuesta = await _httpClientFactory.CreateClient("BarberApi").PutAsJsonAsync($"peluqueros/{id}", peluquero);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el peluquero.");
                return View(peluquero);
            }
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(peluquero);
        }

        TempData["Mensaje"] = "Peluquero actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var respuesta = await _httpClientFactory.CreateClient("BarberApi").DeleteAsync($"peluqueros/{id}");
            TempData["Mensaje"] = respuesta.IsSuccessStatusCode ? "Peluquero eliminado correctamente." : "No se pudo eliminar el peluquero.";
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo conectar con la API.";
        }

        return RedirectToAction(nameof(Index));
    }
}
