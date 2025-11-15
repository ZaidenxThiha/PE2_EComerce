namespace PE2.ECommerce.Domain.Entities;

public class OrderDetail
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitAmount { get; set; }

    public Order? Order { get; set; }
    public Item? Item { get; set; }
}
