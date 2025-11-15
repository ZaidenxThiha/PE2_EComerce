using System.Collections.Generic;
using System.Threading.Tasks;
using PE2.ECommerce.Domain.Entities;

namespace PE2.ECommerce.Services.Interfaces;

public interface IOrderService
{
    Task<Order?> GetAsync(int orderId);
    Task<Order> CreateAsync(Order order);
    Task<IReadOnlyCollection<Order>> GetRecentAsync(int take = 50);
}
