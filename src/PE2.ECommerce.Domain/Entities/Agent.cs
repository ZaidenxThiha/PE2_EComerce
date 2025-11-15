using System.Collections.Generic;

namespace PE2.ECommerce.Domain.Entities;

public class Agent
{
    public int AgentId { get; set; }
    public string AgentName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
