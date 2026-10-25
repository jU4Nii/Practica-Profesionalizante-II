using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace BarberManager.Web.Filters;

public class RequiereSesionAttribute : Attribute, IAuthorizationFilter
{
    public virtual void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Session.GetString("UsuarioId") != null)
            return;

        context.Result = new RedirectToActionResult("Login", "Home", null);
    }
}

public class SoloAdminAttribute : RequiereSesionAttribute
{
    public override void OnAuthorization(AuthorizationFilterContext context)
    {
        base.OnAuthorization(context);
        if (context.Result != null)
            return;

        if (context.HttpContext.Session.GetString("EsAdmin") != "true")
        {
            var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();
            var tempData = factory?.GetTempData(context.HttpContext);
            if (tempData != null)
            {
                tempData["Mensaje"] = "No tenés permisos para acceder a esta sección.";
            }
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
