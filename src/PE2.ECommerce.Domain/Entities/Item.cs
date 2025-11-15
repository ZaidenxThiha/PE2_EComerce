using System.Collections.Generic;

namespace PE2.ECommerce.Domain.Entities;

public class Item
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public int UnitInStock { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
