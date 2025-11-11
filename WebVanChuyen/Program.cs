using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebVanChuyen.Data;
using WebVanChuyen.Interfaces;
using WebVanChuyen.Models;
using WebVanChuyen.Services;
using WebVanChuyen.Utilities;

var builder = WebApplication.CreateBuilder(args);

// 1. L?y chu?i k?t n?i
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. C?u h?nh SQL Server Connection (S? d?ng AddDbContext)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 1. Thêm Identity Service
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Tùy ch?n yêu c?u xác th?c email, v.v.
    options.SignIn.RequireConfirmedAccount = false; // Ð?t thành true n?u c?n xác nh?n email
})
// 2. Liên k?t Identity v?i EF Core và AppDbContext
.AddEntityFrameworkStores<AppDbContext>()
// 3. Thêm Token Providers m?c ð?nh (thý?ng c?n cho reset password)
.AddDefaultTokenProviders();

// 3. Ðãng k? D?ch v? (DI)
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<IPricingService, PricingService>();

// 3. Ðãng k? AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>()); // S?a ð?i t?i ðây

// Thêm các d?ch v? khác (Controllers, Views...)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// *** G?I SEEDER ? ÐÂY ***
// S? d?ng IServiceScopeFactory ð? l?y các d?ch v?
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Ch?y migration (t?o DB n?u chýa có)
        await context.Database.MigrateAsync();

        // Ch?y Seeder
        await DbInitializer.SeedRolesAndAdminAsync(roleManager, userManager);
        await DbInitializer.SeedPricingDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the DB.");
    }
}
// *** K?T THÚC G?I SEEDER ***

// ... (C?u h?nh pipeline: UseStaticFiles, UseRouting, UseAuthentication, UseAuthorization)

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// B?t Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // C?n cho các trang Identity

app.Run();