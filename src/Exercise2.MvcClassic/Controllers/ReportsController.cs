using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise2.MvcClassic.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly IReportingService _reportingService;

    public ReportsController(IReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    public async Task<IActionResult> Index(int? month, int? year)
    {
        var selectedMonth = month ?? DateTime.Today.Month;
        var selectedYear = year ?? DateTime.Today.Year;
        ViewBag.Month = selectedMonth;
        ViewBag.Year = selectedYear;
        ViewBag.Orders = await _reportingService.GetOrderSummariesAsync(selectedMonth, selectedYear);
        ViewBag.Items = await _reportingService.GetBestItemsAsync();
        ViewBag.Agents = await _reportingService.GetAgentPurchasesAsync();
        return View();
    }
}
