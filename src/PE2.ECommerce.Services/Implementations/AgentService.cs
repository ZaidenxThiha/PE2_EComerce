using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PE2.ECommerce.Domain.Data;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace PE2.ECommerce.Services.Implementations;

public class AgentService(ECommerceDbContext dbContext) : IAgentService
{
    public async Task<IReadOnlyCollection<Agent>> GetAsync()
        => await dbContext.Agents.OrderBy(a => a.AgentName).ToListAsync();

    public async Task<Agent?> GetAsync(int id)
        => await dbContext.Agents.FindAsync(id);

    public async Task<Agent> SaveAsync(Agent agent)
    {
        if (agent.AgentId == 0)
        {
            dbContext.Agents.Add(agent);
            await dbContext.SaveChangesAsync();
            return agent;
        }

        var existing = await dbContext.Agents.FirstOrDefaultAsync(a => a.AgentId == agent.AgentId);
        if (existing is null)
        {
            throw new InvalidOperationException($"Agent {agent.AgentId} not found.");
        }

        existing.AgentName = agent.AgentName;
        existing.Address = agent.Address;
        existing.Phone = agent.Phone;
        existing.Email = agent.Email;

        await dbContext.SaveChangesAsync();
        return existing;
    }
}
