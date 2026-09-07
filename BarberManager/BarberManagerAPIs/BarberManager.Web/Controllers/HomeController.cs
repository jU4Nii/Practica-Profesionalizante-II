using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BarberManager.Web.Models;

namespace BarberManager.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    // Por ahora el login es solamente visual. La API todavía no tiene autenticación.
    [HttpPost]
    public IActionResult Login(string? correo, string? contrasena)
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult EnConstruccion(string nombre)
    {
        ViewData["NombreSeccion"] = nombre;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
