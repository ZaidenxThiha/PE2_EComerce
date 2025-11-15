using System.Collections.Generic;
using System.Threading.Tasks;
using PE2.ECommerce.Domain.Entities;

namespace PE2.ECommerce.Services.Interfaces;

public interface IItemService
{
    Task<IReadOnlyCollection<Item>> GetAsync();
    Task<Item?> GetAsync(int id);
    Task<Item> SaveAsync(Item item);
    Task<bool> DeleteAsync(int id);
}
