using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebVanChuyen.Models;

namespace WebVanChuyen.Data
{
    // Kế thừa từ IdentityDbContext<ApplicationUser>
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Thêm các DbSet cho tất cả Models
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentHistory> ShipmentHistories { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<PricingRule> PricingRules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // BẮT BUỘC cho Identity

            // 1. Cấu hình Khóa ngoại và OnDelete = Restrict cho PricingRule
            builder.Entity<PricingRule>()
                .HasOne(pr => pr.FromZone)
                .WithMany() // Không cần navigation ngược lại
                .HasForeignKey(pr => pr.FromZoneId)
                .OnDelete(DeleteBehavior.Restrict); // Đặt OnDelete là Restrict

            builder.Entity<PricingRule>()
                .HasOne(pr => pr.ToZone)
                .WithMany()
                .HasForeignKey(pr => pr.ToZoneId)
                .OnDelete(DeleteBehavior.Restrict); // Đặt OnDelete là Restrict

            // 2. Tạo Unique Index cho TrackingNumber
            builder.Entity<Shipment>()
                .HasIndex(s => s.TrackingNumber)
                .IsUnique();

            // 3. Thiết lập quan hệ một-nhiều giữa ApplicationUser và Shipment
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Shipments) // Một User có nhiều Shipment
                .WithOne(s => s.Sender)    // Một Shipment thuộc về một User (Sender)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Đặt Restrict (hoặc SetNull tùy logic)
        }
    }
}
