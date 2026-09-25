using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using BarberManager.Web.Models;

namespace BarberManager.Web.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("UsuarioId") == null)
            return RedirectToAction(nameof(Login));

        return View();
    }

    public IActionResult Login()
    {
        return HttpContext.Session.GetString("UsuarioId") != null
            ? RedirectToAction(nameof(Index))
            : View(new LoginViewModel());
    }

    [HttpGet]
    public IActionResult CrearCuenta() => View();

    [HttpGet]
    public IActionResult RestablecerContrasena() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel login)
    {
        if (!ModelState.IsValid) return View(login);

        try
        {
            var respuesta = await _httpClientFactory.CreateClient("BarberApi").PostAsJsonAsync("auth/login", login);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos, o la cuenta está inactiva.");
                return View(login);
            }

            var usuario = await respuesta.Content.ReadFromJsonAsync<UsuarioAutenticadoViewModel>();
            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "No se pudo iniciar sesión.");
                return View(login);
            }

            if (string.IsNullOrWhiteSpace(usuario.Token))
            {
                ModelState.AddModelError(string.Empty, "La API no devolvió una sesión válida.");
                return View(login);
            }

            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
            HttpContext.Session.SetString("EsAdmin", usuario.EsAdmin.ToString().ToLowerInvariant());
            HttpContext.Session.SetString("ApiToken", usuario.Token);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
            return View(login);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
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
