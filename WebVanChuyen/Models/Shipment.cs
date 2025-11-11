using System.ComponentModel.DataAnnotations.Schema;

namespace WebVanChuyen.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public string SenderName { get; set; }
        public string ReceiverAddress { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal WeightKg { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal ShippingFee { get; set; } // Phí vận chuyển

        public ShipmentStatus CurrentStatus { get; set; } // Sử dụng Enum
        public DateTime CreatedDate { get; set; } // Thêm theo yêu cầu 5.1

        // Khóa ngoại và Navigation Property cho Sender (ApplicationUser)
        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; }

        // Navigation Property (một Shipment có nhiều History)
        public virtual ICollection<ShipmentHistory> History { get; set; }
    }
}
