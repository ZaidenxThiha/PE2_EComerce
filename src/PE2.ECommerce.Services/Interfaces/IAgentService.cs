using System.Collections.Generic;
using System.Threading.Tasks;
using PE2.ECommerce.Domain.Entities;

namespace PE2.ECommerce.Services.Interfaces;

public interface IAgentService
{
    Task<IReadOnlyCollection<Agent>> GetAsync();
    Task<Agent?> GetAsync(int id);
    Task<Agent> SaveAsync(Agent agent);
}
