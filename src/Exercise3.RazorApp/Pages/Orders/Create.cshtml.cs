using Exercise3.RazorApp.Infrastructure;
using Exercise3.RazorApp.Services;
using Microsoft.AspNetCore.Mvc;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise3.RazorApp.Pages.Orders;

public class CreateModel : SecuredPageModel
{
    private readonly IAgentService _agentService;
    private readonly IItemService _itemService;
    private readonly IOrderService _orderService;

    public CreateModel(IUserSession session, IAgentService agentService, IItemService itemService, IOrderService orderService) : base(session)
    {
        _agentService = agentService;
        _itemService = itemService;
        _orderService = orderService;
    }

    public IReadOnlyCollection<Agent> Agents { get; private set; } = Array.Empty<Agent>();
    public IReadOnlyCollection<Item> Items { get; private set; } = Array.Empty<Item>();

    [BindProperty]
    public OrderInput Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        await LoadReferenceDataAsync();
        EnsureLinePlaceholders();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        await LoadReferenceDataAsync();
        EnsureLinePlaceholders();

        var lines = Input.Lines
            .Where(l => l.ItemId > 0 && l.Quantity > 0)
            .Select(l => new OrderDetail
            {
                ItemId = l.ItemId,
                Quantity = l.Quantity,
                UnitAmount = Items.First(i => i.ItemId == l.ItemId).UnitPrice
            })
            .ToList();

        if (!lines.Any())
        {
            ModelState.AddModelError(string.Empty, "Add at least one line");
            return Page();
        }

        var userId = UserSession.UserId ?? throw new InvalidOperationException("Session expired");

        var order = new Order
        {
            AgentId = Input.AgentId,
            OrderDate = Input.OrderDate,
            CreatedBy = userId,
            Notes = Input.Notes,
            Details = lines
        };

        await _orderService.CreateAsync(order);
        TempData["Status"] = $"Order {order.OrderId} saved";
        return RedirectToPage("/Index");
    }

    private async Task LoadReferenceDataAsync()
    {
        Agents = await _agentService.GetAsync();
        Items = await _itemService.GetAsync();
    }

    private void EnsureLinePlaceholders()
    {
        while (Input.Lines.Count < 5)
        {
            Input.Lines.Add(new OrderLineInput());
        }
    }

    public class OrderInput
    {
        public int AgentId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Today;
        public string? Notes { get; set; }
        public List<OrderLineInput> Lines { get; set; } = new();
    }

    public class OrderLineInput
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
