using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebVanChuyen.Models;

namespace WebVanChuyen.Data
{
    public static class DbInitializer
    {
        // Hàm Seeding Roles và Admin
        public static async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            // 1. Tạo 3 Roles: 'Admin', 'Staff', 'Customer'
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Staff"))
            {
                await roleManager.CreateAsync(new IdentityRole("Staff"));
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            // 2. Tạo tài khoản Admin mặc định
            var adminEmail = "admin@btl.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Quản Trị Viên",
                    EmailConfirmed = true // Xác thực email ngay
                };

                // Tạo user với mật khẩu "Admin123!"
                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    // 3. Gán Role 'Admin' cho tài khoản
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        // (Thêm các hàm Seed khác ở Bước 4)
        // Hàm Seeding Pricing Data
        public static async Task SeedPricingDataAsync(AppDbContext context)
        {
            // 1. Kiểm tra Zones
            if (!await context.Zones.AnyAsync())
            {
                var zones = new List<Zone>
                {
                    new Zone { ZoneName = "Zone A" }, // Id = 1
                    new Zone { ZoneName = "Zone B" }, // Id = 2
                    new Zone { ZoneName = "Zone C" }  // Id = 3
                };
                await context.Zones.AddRangeAsync(zones);
                await context.SaveChangesAsync(); // Lưu để lấy IDs
            }

            // 2. Kiểm tra PricingRules
            if (!await context.PricingRules.AnyAsync())
            {
                var rules = new List<PricingRule>
                {
                    // Zone A -> A (0-2kg, 2-5kg)
                    new PricingRule { FromZoneId = 1, ToZoneId = 1, MinWeightKg = 0, MaxWeightKg = 2, BaseFee = 25000, ExtraFeePerKg = 0 },
                    new PricingRule { FromZoneId = 1, ToZoneId = 1, MinWeightKg = 2, MaxWeightKg = 5, BaseFee = 35000, ExtraFeePerKg = 5000 },
                    // Zone A -> B (0-3kg)
                    new PricingRule { FromZoneId = 1, ToZoneId = 2, MinWeightKg = 0, MaxWeightKg = 3, BaseFee = 40000, ExtraFeePerKg = 3000 },
                    // Zone B -> C (0-5kg)
                    new PricingRule { FromZoneId = 2, ToZoneId = 3, MinWeightKg = 0, MaxWeightKg = 5, BaseFee = 50000, ExtraFeePerKg = 2000 }
                };
                await context.PricingRules.AddRangeAsync(rules);
                await context.SaveChangesAsync();
            }
        }
    }
}
