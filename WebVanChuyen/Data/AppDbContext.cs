using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebVanChuyen.Models;

namespace WebVanChuyen.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Các DbSet
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentHistory> ShipmentHistories { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<PricingRule> PricingRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Tạo Unique Index cho TrackingNumber trong Shipment
            modelBuilder.Entity<Shipment>()
                .HasIndex(s => s.TrackingNumber)
                .IsUnique();

            // 2. Thiết lập quan hệ ApplicationUser - Shipment (một-nhiều)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.SentShipments)
                .WithOne(s => s.Sender)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Giả sử muốn giữ đơn hàng khi User bị xóa (hoặc Restrict)

            // 3. Thiết lập quan hệ Khóa ngoại cho PricingRule và OnDelete = Restrict
            // Từ Zone (FromZoneId)
            modelBuilder.Entity<PricingRule>()
                .HasOne(pr => pr.FromZone)
                .WithMany() // Không cần Navigation Collection trong Zone cho From/To
                .HasForeignKey(pr => pr.FromZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            // Đến Zone (ToZoneId)
            modelBuilder.Entity<PricingRule>()
                .HasOne(pr => pr.ToZone)
                .WithMany()
                .HasForeignKey(pr => pr.ToZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            // Đảm bảo tên bảng Identity không bị CamelCase nếu không cần thiết
            // foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            // {
            //     var tableName = entityType.GetTableName();
            //     if (tableName.StartsWith("AspNet"))
            //     {
            //         entityType.SetTableName(tableName.Replace("AspNet", ""));
            //     }
            // }
        }
    }
}
