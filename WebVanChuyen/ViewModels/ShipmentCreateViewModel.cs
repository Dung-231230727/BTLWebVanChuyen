using System.ComponentModel.DataAnnotations;

namespace WebVanChuyen.ViewModels
{
    // Lớp VM này được tạo để phục vụ cho Yêu cầu
    public class ShipmentCreateViewModel
    {
        [Required]
        public string SenderName { get; set; }

        [Required]
        public string ReceiverAddress { get; set; }

        [Required]
        [Range(0.1, 100)]
        public decimal WeightKg { get; set; }

        [Required]
        public int FromZoneId { get; set; }

        [Required]
        public int ToZoneId { get; set; }

        // Thêm các thuộc tính khác của Shipment nếu cần (ReceiverName, v.v.)
    }
}
