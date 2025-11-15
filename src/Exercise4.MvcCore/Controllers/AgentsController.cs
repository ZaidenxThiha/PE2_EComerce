using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise4.MvcCore.Controllers;

[Authorize]
public class AgentsController : Controller
{
    private readonly IAgentService _agentService;

    public AgentsController(IAgentService agentService)
    {
        _agentService = agentService;
    }

    public async Task<IActionResult> Index()
    {
        var agents = await _agentService.GetAsync();
        return View(agents);
    }

    public IActionResult Create() => View(new Agent());

    [HttpPost]
    public async Task<IActionResult> Create(Agent agent)
    {
        if (!ModelState.IsValid)
        {
            return View(agent);
        }

        await _agentService.SaveAsync(agent);
        return RedirectToAction(nameof(Index));
    }
}
