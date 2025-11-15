using System.Collections.Generic;
using System.Threading.Tasks;
using PE2.ECommerce.Services.Models;

namespace PE2.ECommerce.Services.Interfaces;

public interface IReportingService
{
    Task<IReadOnlyCollection<OrderSummaryModel>> GetOrderSummariesAsync(int month, int year);
    Task<IReadOnlyCollection<ItemPerformanceModel>> GetBestItemsAsync(int top = 10);
    Task<IReadOnlyCollection<AgentPurchaseModel>> GetAgentPurchasesAsync(int top = 10);
}
