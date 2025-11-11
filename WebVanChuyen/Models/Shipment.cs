using System.ComponentModel.DataAnnotations.Schema;

namespace WebVanChuyen.Models
{
    public enum ShipmentStatus
    {
        Pending,     // Đang chờ xác nhận
        Processing,  // Đang xử lý (nhận hàng)
        InTransit,   // Đang vận chuyển
        Delivered,   // Đã giao hàng
        Canceled     // Đã hủy
    }
    public class Shipment
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public string SenderName { get; set; } // Tên người gửi (hoặc có thể dùng ApplicationUser.FullName)
        public string ReceiverAddress { get; set; }
        public decimal WeightKg { get; set; }
        public decimal ShippingFee { get; set; }
        public ShipmentStatus CurrentStatus { get; set; }

        // Foreign Key: Sender (User tạo đơn hàng)
        public string SenderId { get; set; }

        // Navigation Properties
        [ForeignKey("SenderId")]
        public ApplicationUser Sender { get; set; }
        public ICollection<ShipmentHistory> History { get; set; }
    }
}
