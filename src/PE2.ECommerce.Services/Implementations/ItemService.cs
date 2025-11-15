using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PE2.ECommerce.Domain.Data;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace PE2.ECommerce.Services.Implementations;

public class ItemService(ECommerceDbContext dbContext) : IItemService
{
    public async Task<IReadOnlyCollection<Item>> GetAsync()
        => await dbContext.Items.OrderBy(i => i.ItemName).ToListAsync();

    public async Task<Item?> GetAsync(int id)
        => await dbContext.Items.FindAsync(id);

    public async Task<Item> SaveAsync(Item item)
    {
        if (item.ItemId == 0)
        {
            dbContext.Items.Add(item);
            await dbContext.SaveChangesAsync();
            return item;
        }

        var existing = await dbContext.Items.FirstOrDefaultAsync(i => i.ItemId == item.ItemId);
        if (existing is null)
        {
            throw new InvalidOperationException($"Item {item.ItemId} not found.");
        }

        existing.ItemName = item.ItemName;
        existing.Size = item.Size;
        existing.UnitInStock = item.UnitInStock;
        existing.UnitPrice = item.UnitPrice;
        existing.IsActive = item.IsActive;

        await dbContext.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await dbContext.Items.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        dbContext.Items.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
