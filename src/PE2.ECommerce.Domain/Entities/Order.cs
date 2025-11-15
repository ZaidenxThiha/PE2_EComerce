using System;
using System.Collections.Generic;
using System.Linq;

namespace PE2.ECommerce.Domain.Entities;

public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public int AgentId { get; set; }
    public int CreatedBy { get; set; }
    public string? Notes { get; set; }

    public Agent? Agent { get; set; }
    public AppUser? User { get; set; }
    public ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();

    public decimal Total => Details.Sum(d => d.Quantity * d.UnitAmount);
}
