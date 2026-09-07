using System.Net.Http.Json;
using BarberManager.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarberManager.Web.Controllers;

public class ProductosController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductosController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var productos = await api.GetFromJsonAsync<List<ProductoViewModel>>("productos") ?? [];
            return View(productos.OrderBy(p => p.Nombre).ToList());
        }
        catch (HttpRequestException)
        {
            ViewBag.ErrorApi = "No se pudo conectar con la API.";
            return View(new List<ProductoViewModel>());
        }
    }

    public IActionResult Create() => View(new ProductoViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductoViewModel producto)
    {
        if (!ModelState.IsValid) return View(producto);

        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var respuesta = await api.PostAsJsonAsync("productos", producto);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo crear el producto.");
                return View(producto);
            }

            TempData["Mensaje"] = "Producto creado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(producto);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var producto = await api.GetFromJsonAsync<ProductoViewModel>($"productos/{id}");
            return producto is null ? NotFound() : View(producto);
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo conectar con la API.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductoViewModel producto)
    {
        if (!ModelState.IsValid) return View(producto);

        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var respuesta = await api.PutAsJsonAsync($"productos/{id}", producto);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el producto.");
                return View(producto);
            }

            TempData["Mensaje"] = "Producto actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(producto);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var respuesta = await api.DeleteAsync($"productos/{id}");
            TempData["Mensaje"] = respuesta.IsSuccessStatusCode ? "Producto eliminado correctamente." : "No se pudo eliminar el producto.";
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo conectar con la API.";
        }

        return RedirectToAction(nameof(Index));
    }
}
