using Microsoft.EntityFrameworkCore;
using PE2.ECommerce.Domain.Entities;

namespace PE2.ECommerce.Domain.Data;

public class ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : DbContext(options)
{
    public const string DefaultConnectionName = "ECommerceDb";
    public const string PipeServer = "np://./pipe/LOCALDB#20734081/tsql/query";

    public DbSet<Item> Items => Set<Item>();
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");
            entity.Property(p => p.ItemName).IsRequired().HasMaxLength(120);
            entity.Property(p => p.Size).HasMaxLength(30);
            entity.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Agent>(entity =>
        {
            entity.ToTable("Agent");
            entity.Property(p => p.AgentName).IsRequired().HasMaxLength(120);
            entity.Property(p => p.Address).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Phone).HasMaxLength(30);
            entity.Property(p => p.Email).HasMaxLength(120);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(p => p.UserId);
            entity.Property(p => p.UserName).HasMaxLength(50);
            entity.Property(p => p.Email).HasMaxLength(100);
            entity.Property(p => p.RoleName).HasMaxLength(30);
            entity.Property(p => p.PasswordHash).HasMaxLength(200);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");
            entity.Property(p => p.OrderDate).HasColumnType("datetime2");
            entity.Property(p => p.Notes).HasMaxLength(200);
            entity.HasOne(o => o.Agent).WithMany(a => a.Orders).HasForeignKey(o => o.AgentId);
            entity.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.CreatedBy);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");
            entity.Property(p => p.UnitAmount).HasColumnType("decimal(18,2)");
            entity.HasOne(d => d.Order).WithMany(o => o.Details).HasForeignKey(d => d.OrderId);
            entity.HasOne(d => d.Item).WithMany(i => i.OrderDetails).HasForeignKey(d => d.ItemId);
            entity.HasIndex(d => new { d.OrderId, d.ItemId }).IsUnique();
        });
    }
}
