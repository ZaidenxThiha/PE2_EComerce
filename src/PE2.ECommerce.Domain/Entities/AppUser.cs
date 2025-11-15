using System.Collections.Generic;

namespace PE2.ECommerce.Domain.Entities;

public class AppUser
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool Lock { get; set; }
    public string RoleName { get; set; } = "Staff";

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
