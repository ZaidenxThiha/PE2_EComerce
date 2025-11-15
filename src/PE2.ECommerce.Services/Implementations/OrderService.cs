using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PE2.ECommerce.Domain.Data;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace PE2.ECommerce.Services.Implementations;

public class OrderService(ECommerceDbContext dbContext) : IOrderService
{
    public async Task<Order?> GetAsync(int orderId)
        => await dbContext.Orders
            .Include(o => o.Agent)
            .Include(o => o.Details)
            .Include(o => o.User)
            .Include(o => o.Details)
                .ThenInclude(d => d.Item)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

    public async Task<Order> CreateAsync(Order order)
    {
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        return order;
    }

    public async Task<IReadOnlyCollection<Order>> GetRecentAsync(int take = 50)
        => await dbContext.Orders
            .Include(o => o.Agent)
            .Include(o => o.Details)
                .ThenInclude(d => d.Item)
            .OrderByDescending(o => o.OrderDate)
            .Take(take)
            .ToListAsync();
}
