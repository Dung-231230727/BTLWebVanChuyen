using System.Threading.Tasks;

namespace WebVanChuyen.Interfaces
{
    public interface IPricingService
    {
        Task<decimal> CalculateFeeAsync(decimal weightKg, int fromZoneId, int toZoneId);
    }
}