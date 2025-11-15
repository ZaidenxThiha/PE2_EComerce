using System.ComponentModel.DataAnnotations;
using Exercise3.RazorApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise3.RazorApp.Pages;

public class LoginModel : PageModel
{
    private readonly IAuthService _authService;
    private readonly IUserSession _session;

    public LoginModel(IAuthService authService, IUserSession session)
    {
        _authService = authService;
        _session = session;
    }

    [BindProperty]
    public CredentialInput Input { get; set; } = new();

    public string? Error { get; set; }

    public IActionResult OnGet()
    {
        if (_session.IsAuthenticated)
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _authService.LoginAsync(Input.UserName, Input.Password);
        if (!result.Success || result.UserId is null)
        {
            Error = result.Error ?? "Unable to login";
            return Page();
        }

        _session.SignIn(result.UserId.Value, Input.UserName);
        return RedirectToPage("/Index");
    }

    public class CredentialInput
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
