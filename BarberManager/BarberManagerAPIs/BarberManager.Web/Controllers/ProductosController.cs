using System.Net.Http.Json;
using BarberManager.Web.Models;
using BarberManager.Web.Filters;
using BarberManager.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BarberManager.Web.Controllers;

[RequiereSesion]
public class ProductosController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductosController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            var productos = await api.GetFromJsonAsync<List<ProductoViewModel>>("productos") ?? [];
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                productos = productos.Where(p =>
                    p.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase) ||
                    p.Descripcion.Contains(buscar, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            ViewData["Buscar"] = buscar;
            return View(productos.OrderBy(p => p.Nombre).ToList());
        }
        catch (HttpRequestException)
        {
            ViewBag.ErrorApi = "No se pudo conectar con la API.";
            return View(new List<ProductoViewModel>());
        }
    }

    public IActionResult Create() => View(new ProductoViewModel());

    public async Task<IActionResult> ExportarCsv()
    {
        try
        {
            var productos = await _httpClientFactory.CreateClient("BarberApi")
                .GetFromJsonAsync<List<ProductoViewModel>>("productos") ?? [];
            var csv = CsvExportador.Crear(
                ["Id", "Producto", "Descripcion", "Cantidad", "Uso", "Precio"],
                productos.Select(p => new[]
                {
                    p.Id.ToString(), p.Nombre, p.Descripcion, p.Cantidad.ToString(),
                    p.UsoInterno ? "Interno" : "Venta", p.Precio.ToString(System.Globalization.CultureInfo.InvariantCulture)
                }));
            return File(csv, "text/csv", "productos.csv");
        }
        catch (HttpRequestException)
        {
            TempData["Mensaje"] = "No se pudo generar el respaldo de productos.";
            return RedirectToAction(nameof(Index));
        }
    }

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
