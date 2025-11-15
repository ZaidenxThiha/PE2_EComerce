using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise2.MvcClassic.Controllers;

[Authorize]
public class ItemsController : Controller
{
    private readonly IItemService _itemService;

    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _itemService.GetAsync();
        return View(items);
    }

    public IActionResult Create() => View(new Item());

    [HttpPost]
    public async Task<IActionResult> Create(Item item)
    {
        if (!ModelState.IsValid)
        {
            return View(item);
        }

        await _itemService.SaveAsync(item);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _itemService.GetAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Item item)
    {
        if (id != item.ItemId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(item);
        }

        await _itemService.SaveAsync(item);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _itemService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
