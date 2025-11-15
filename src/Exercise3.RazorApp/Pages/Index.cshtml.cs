using Exercise3.RazorApp.Infrastructure;
using Exercise3.RazorApp.Services;
using Microsoft.AspNetCore.Mvc;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise3.RazorApp.Pages;

public class IndexModel : SecuredPageModel
{
    private readonly IOrderService _orderService;

    public IndexModel(IUserSession session, IOrderService orderService) : base(session)
    {
        _orderService = orderService;
    }

    public IReadOnlyCollection<Order> RecentOrders { get; private set; } = Array.Empty<Order>();
    public IUserSession SessionState => UserSession;

    public async Task<IActionResult> OnGetAsync()
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        RecentOrders = await _orderService.GetRecentAsync(10);
        return Page();
    }
}
