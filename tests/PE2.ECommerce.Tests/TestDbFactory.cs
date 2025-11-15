using Microsoft.EntityFrameworkCore;
using PE2.ECommerce.Domain.Data;
using PE2.ECommerce.Domain.Entities;

namespace PE2.ECommerce.Tests;

public static class TestDbFactory
{
    public static ECommerceDbContext CreateContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<ECommerceDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
        var context = new ECommerceDbContext(options);
        if (!context.Users.Any())
        {
            Seed(context);
        }
        return context;
    }

    private static void Seed(ECommerceDbContext context)
    {
        var user = new AppUser
        {
            UserName = "tester",
            Email = "tester@example.com",
            PasswordHash = "098f6bcd4621d373cade4e832627b4f6"
        };
        context.Users.Add(user);

        var agent = new Agent { AgentName = "Agent", Address = "Addr", Phone = "000", Email = "agent@example.com" };
        context.Agents.Add(agent);

        var item = new Item { ItemName = "Sample", Size = "1kg", UnitInStock = 10, UnitPrice = 5m };
        context.Items.Add(item);
        context.SaveChanges();

        var order = new Order
        {
            AgentId = agent.AgentId,
            CreatedBy = user.UserId,
            OrderDate = DateTime.Today,
            Details = new List<OrderDetail>
            {
                new() { ItemId = item.ItemId, Quantity = 2, UnitAmount = 5m }
            }
        };
        context.Orders.Add(order);
        context.SaveChanges();
    }
}
