using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PE2.ECommerce.Domain.Data;
using PE2.ECommerce.Services.Interfaces;
using PE2.ECommerce.Services.Models;

namespace PE2.ECommerce.Services.Implementations;

public class ReportingService(ECommerceDbContext dbContext) : IReportingService
{
    public async Task<IReadOnlyCollection<OrderSummaryModel>> GetOrderSummariesAsync(int month, int year)
    {
        var orders = await dbContext.Orders
            .Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year)
            .Include(o => o.Agent)
            .Include(o => o.User)
            .Include(o => o.Details)
            .AsNoTracking()
            .ToListAsync();

        return orders
            .Select(o => new OrderSummaryModel(
                o.OrderId,
                o.OrderDate,
                o.Agent?.AgentName ?? "Unknown",
                o.Details.Sum(d => d.Quantity * d.UnitAmount),
                o.User?.UserName ?? "Unknown"))
            .OrderByDescending(o => o.OrderDate)
            .ToList();
    }

    public async Task<IReadOnlyCollection<ItemPerformanceModel>> GetBestItemsAsync(int top = 10)
    {
        var details = await dbContext.OrderDetails
            .Include(d => d.Item)
            .AsNoTracking()
            .ToListAsync();

        return details
            .GroupBy(d => d.Item?.ItemName ?? "Unknown")
            .Select(g => new ItemPerformanceModel(
                g.Key,
                g.Sum(d => d.Quantity),
                g.Sum(d => d.Quantity * d.UnitAmount)))
            .OrderByDescending(x => x.QuantitySold)
            .ThenByDescending(x => x.Revenue)
            .Take(top)
            .ToList();
    }

    public async Task<IReadOnlyCollection<AgentPurchaseModel>> GetAgentPurchasesAsync(int top = 10)
    {
        var orders = await dbContext.Orders
            .Include(o => o.Agent)
            .Include(o => o.Details)
            .AsNoTracking()
            .ToListAsync();

        return orders
            .Select(o => new AgentPurchaseModel(
                o.Agent?.AgentName ?? "Unknown",
                o.Details.Sum(d => d.Quantity),
                o.Details.Sum(d => d.Quantity * d.UnitAmount)))
            .OrderByDescending(x => x.TotalAmount)
            .Take(top)
            .ToList();
    }
}
