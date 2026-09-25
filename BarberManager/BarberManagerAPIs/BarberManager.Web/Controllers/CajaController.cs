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

    public async Task<IActionResult> Create()
    {
        var movimiento = new CajaViewModel();
        await CargarProductosDisponibles(movimiento);
        return View(movimiento);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CajaViewModel movimiento)
    {
        try
        {
            var api = _httpClientFactory.CreateClient("BarberApi");
            HttpResponseMessage respuesta;

            if (movimiento.EsIngreso && movimiento.TipoIngreso == "Venta de producto")
            {
                ModelState.Remove(nameof(movimiento.Monto));
                ModelState.Remove(nameof(movimiento.Concepto));
                if (!movimiento.IdProducto.HasValue)
                    ModelState.AddModelError(nameof(movimiento.IdProducto), "Seleccioná un producto.");
                if (movimiento.CantidadProducto <= 0)
                    ModelState.AddModelError(nameof(movimiento.CantidadProducto), "La cantidad debe ser mayor a cero.");

                if (!ModelState.IsValid)
                {
                    await CargarProductosDisponibles(movimiento);
                    return View(movimiento);
                }

                respuesta = await api.PostAsJsonAsync("caja/venta-producto", movimiento);
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    await CargarProductosDisponibles(movimiento);
                    return View(movimiento);
                }

                respuesta = await api.PostAsJsonAsync("caja", movimiento);
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, movimiento.EsIngreso && movimiento.TipoIngreso == "Venta de producto"
                    ? "No se pudo registrar la venta. Verificá el stock disponible."
                    : "No se pudo registrar el movimiento.");
                await CargarProductosDisponibles(movimiento);
                return View(movimiento);
            }
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            await CargarProductosDisponibles(movimiento);
            return View(movimiento);
        }

        TempData["Mensaje"] = "Movimiento registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarProductosDisponibles(CajaViewModel movimiento)
    {
        try
        {
            var productos = await _httpClientFactory.CreateClient("BarberApi")
                .GetFromJsonAsync<List<ProductoViewModel>>("productos") ?? [];
            movimiento.ProductosDisponibles = productos
                .Where(p => !p.UsoInterno && p.Cantidad > 0)
                .OrderBy(p => p.Nombre)
                .Select(p => new ProductoDisponibleVentaViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Cantidad = p.Cantidad,
                    Precio = p.Precio
                })
                .ToList();
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los productos disponibles.");
        }
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
