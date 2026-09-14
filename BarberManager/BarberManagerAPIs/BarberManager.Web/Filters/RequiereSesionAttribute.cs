using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
            context.Result = new RedirectToActionResult("Index", "Home", null);
    }
}
