using Exercise4.MvcCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise4.MvcCore.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IAgentService _agentService;
    private readonly IItemService _itemService;
    private readonly IOrderService _orderService;

    public OrdersController(IAgentService agentService, IItemService itemService, IOrderService orderService)
    {
        _agentService = agentService;
        _itemService = itemService;
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetRecentAsync();
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadReferenceDataAsync();
        return View(new OrderViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderViewModel model)
    {
        await LoadReferenceDataAsync();
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var validLines = model.Lines.Where(l => l.ItemId > 0 && l.Quantity > 0).ToList();
        if (!validLines.Any())
        {
            ModelState.AddModelError(string.Empty, "Add at least one line");
            return View(model);
        }

        var items = await _itemService.GetAsync();
        var order = new Order
        {
            AgentId = model.AgentId,
            OrderDate = model.OrderDate,
            Notes = model.Notes,
            CreatedBy = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value),
            Details = validLines.Select(line => new OrderDetail
            {
                ItemId = line.ItemId,
                Quantity = line.Quantity,
                UnitAmount = items.First(i => i.ItemId == line.ItemId).UnitPrice
            }).ToList()
        };

        await _orderService.CreateAsync(order);
        return RedirectToAction(nameof(Print), new { id = order.OrderId });
    }

    public async Task<IActionResult> Print(int id)
    {
        var order = await _orderService.GetAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        return View(order);
    }

    private async Task LoadReferenceDataAsync()
    {
        ViewBag.Agents = new SelectList(await _agentService.GetAsync(), nameof(Agent.AgentId), nameof(Agent.AgentName));
        ViewBag.Items = new SelectList(await _itemService.GetAsync(), nameof(Item.ItemId), nameof(Item.ItemName));
    }
}
