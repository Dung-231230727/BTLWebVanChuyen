using Microsoft.EntityFrameworkCore;
using WebVanChuyen.Data;
using WebVanChuyen.Interfaces;

namespace WebVanChuyen.Services
{
    public class PricingService : IPricingService
    {
        private readonly AppDbContext _context;

        // Tiêm AppDbContext
        public PricingService(AppDbContext context)
        {
            _context = context;
        }

        // Hoàn thiện logic tính phí
        public async Task<decimal> CalculateFeeAsync(decimal weightKg, int fromZoneId, int toZoneId)
        {
            // 1. Tìm PricingRule phù hợp
            var rule = await _context.PricingRules
                .FirstOrDefaultAsync(r =>
                    r.FromZoneId == fromZoneId &&
                    r.ToZoneId == toZoneId &&
                    weightKg >= r.MinWeightKg &&
                    weightKg <= r.MaxWeightKg);

            // 3. Nếu không tìm thấy Rule
            if (rule == null)
            {
                throw new InvalidOperationException("Không tìm thấy quy tắc tính phí phù hợp cho tuyến đường và trọng lượng này.");
            }

            // 2. Áp dụng công thức tính
            // Total Fee = BaseFee + (weightKg - MinWeightKg) * ExtraFeePerKg
            var extraWeight = weightKg - rule.MinWeightKg;
            var totalFee = rule.BaseFee + (extraWeight * rule.ExtraFeePerKg);

            return totalFee;
        }
    }
}
