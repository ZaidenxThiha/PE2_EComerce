using System.ComponentModel.DataAnnotations;

namespace Exercise4.MvcCore.Models;

public class OrderViewModel
{
    [Required]
    [Display(Name = "Agent")]
    public int AgentId { get; set; }

    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; } = DateTime.Today;

    public string? Notes { get; set; }

    public List<OrderLineViewModel> Lines { get; set; } = new()
    {
        new(), new(), new()
    };
}

public class OrderLineViewModel
{
    [Display(Name = "Item")]
    public int ItemId { get; set; }

    [Range(1, 999)]
    public int Quantity { get; set; } = 1;
}
