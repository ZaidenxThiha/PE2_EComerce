using Exercise3.RazorApp.Infrastructure;
using Exercise3.RazorApp.Services;
using Microsoft.AspNetCore.Mvc;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise3.RazorApp.Pages.Items;

public class IndexModel : SecuredPageModel
{
    private readonly IItemService _itemService;

    public IndexModel(IUserSession session, IItemService itemService) : base(session)
    {
        _itemService = itemService;
    }

    public IReadOnlyCollection<Item> Items { get; private set; } = Array.Empty<Item>();

    [BindProperty]
    public Item Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        Items = await _itemService.GetAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        if (!ModelState.IsValid)
        {
            Items = await _itemService.GetAsync();
            return Page();
        }

        await _itemService.SaveAsync(Input);
        TempData["Status"] = "Item saved";
        return RedirectToPage();
    }
}
