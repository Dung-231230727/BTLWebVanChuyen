using System.ComponentModel.DataAnnotations.Schema;

namespace WebVanChuyen.Models
{
    public class PricingRule
    {
        public int Id { get; set; }

        // Khóa ngoại tới Zone (Gửi từ)
        public int FromZoneId { get; set; }
        [ForeignKey("FromZoneId")]
        public virtual Zone FromZone { get; set; }

        // Khóa ngoại tới Zone (Gửi đến)
        public int ToZoneId { get; set; }
        [ForeignKey("ToZoneId")]
        public virtual Zone ToZone { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal MinWeightKg { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal MaxWeightKg { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BaseFee { get; set; } // Phí cơ bản

        [Column(TypeName = "decimal(18, 2)")]
        public decimal ExtraFeePerKg { get; set; } // Phí cho mỗi Kg vượt mức
    }
}
