namespace WebVanChuyen.Models
{
    public class PricingRule
    {
        public int Id { get; set; }

        // Foreign Keys
        public int FromZoneId { get; set; }
        public int ToZoneId { get; set; }

        public decimal MinWeightKg { get; set; }
        public decimal MaxWeightKg { get; set; }
        public decimal BaseFee { get; set; }
        public decimal ExtraFeePerKg { get; set; }

        // Navigation Properties
        public Zone FromZone { get; set; }
        public Zone ToZone { get; set; }
    }
}
