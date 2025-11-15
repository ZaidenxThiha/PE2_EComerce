using Exercise3.RazorApp.Infrastructure;
using Exercise3.RazorApp.Services;
using Microsoft.AspNetCore.Mvc;
using PE2.ECommerce.Services.Interfaces;
using PE2.ECommerce.Services.Models;

namespace Exercise3.RazorApp.Pages.Reports;

public class IndexModel : SecuredPageModel
{
    private readonly IReportingService _reportingService;

    public IndexModel(IUserSession session, IReportingService reportingService) : base(session)
    {
        _reportingService = reportingService;
    }

    [BindProperty]
    public int Month { get; set; } = DateTime.Today.Month;

    [BindProperty]
    public int Year { get; set; } = DateTime.Today.Year;

    public IReadOnlyCollection<OrderSummaryModel> Orders { get; private set; } = Array.Empty<OrderSummaryModel>();
    public IReadOnlyCollection<ItemPerformanceModel> Items { get; private set; } = Array.Empty<ItemPerformanceModel>();
    public IReadOnlyCollection<AgentPurchaseModel> Agents { get; private set; } = Array.Empty<AgentPurchaseModel>();

    public async Task<IActionResult> OnGetAsync() => await LoadAsync();

    public async Task<IActionResult> OnPostAsync() => await LoadAsync();

    private async Task<IActionResult> LoadAsync()
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        Orders = await _reportingService.GetOrderSummariesAsync(Month, Year);
        Items = await _reportingService.GetBestItemsAsync();
        Agents = await _reportingService.GetAgentPurchasesAsync();
        return Page();
    }
}
