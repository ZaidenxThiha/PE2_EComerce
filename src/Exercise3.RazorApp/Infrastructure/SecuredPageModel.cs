using Exercise3.RazorApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.RazorApp.Infrastructure;

public abstract class SecuredPageModel : PageModel
{
    protected SecuredPageModel(IUserSession userSession)
    {
        UserSession = userSession;
    }

    protected IUserSession UserSession { get; }

    protected IActionResult? RequireLogin()
    {
        if (!UserSession.IsAuthenticated)
        {
            return RedirectToPage("/Login");
        }

        return null;
    }
}
