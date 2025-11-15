using Exercise3.RazorApp.Infrastructure;
using Exercise3.RazorApp.Services;
using Microsoft.AspNetCore.Mvc;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise3.RazorApp.Pages.Agents;

public class IndexModel : SecuredPageModel
{
    private readonly IAgentService _agentService;

    public IndexModel(IUserSession session, IAgentService agentService) : base(session)
    {
        _agentService = agentService;
    }

    public IReadOnlyCollection<Agent> Agents { get; private set; } = Array.Empty<Agent>();

    [BindProperty]
    public Agent Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        Agents = await _agentService.GetAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        await _agentService.SaveAsync(Input);
        return RedirectToPage();
    }
}
