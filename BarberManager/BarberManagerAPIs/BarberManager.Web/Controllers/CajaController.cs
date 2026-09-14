using System.Net.Http.Json;
using BarberManager.Web.Models;
using BarberManager.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BarberManager.Web.Controllers;

[RequiereSesion]
public class CajaController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CajaController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public async Task<IActionResult> Index()
    {
        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var movimientos = await api.GetFromJsonAsync<List<CajaViewModel>>("caja") ?? [];
            return View(movimientos.OrderByDescending(m => m.Fecha).ToList());
        }
        catch (HttpRequestException)
        {
            ViewBag.ErrorApi = "No se pudo conectar con la API.";
            return View(new List<CajaViewModel>());
        }
    }

    public IActionResult Create() => View(new CajaViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CajaViewModel movimiento)
    {
        if (!ModelState.IsValid) return View(movimiento);
        try
        {
            var respuesta = await _httpClientFactory.CreateClient("BarberApi").PostAsJsonAsync("caja", movimiento);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo registrar el movimiento.");
                return View(movimiento);
            }
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(movimiento);
        }

        TempData["Mensaje"] = "Movimiento registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var movimiento = await _httpClientFactory.CreateClient("BarberApi").GetFromJsonAsync<CajaViewModel>($"caja/{id}");
            return movimiento is null ? NotFound() : View(movimiento);
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo conectar con la API.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CajaViewModel movimiento)
    {
        if (!ModelState.IsValid) return View(movimiento);
        try
        {
            var respuesta = await _httpClientFactory.CreateClient("BarberApi").PutAsJsonAsync($"caja/{id}", movimiento);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el movimiento.");
                return View(movimiento);
            }
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(movimiento);
        }

        TempData["Mensaje"] = "Movimiento actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
