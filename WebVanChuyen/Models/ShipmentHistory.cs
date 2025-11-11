using System.ComponentModel.DataAnnotations.Schema;

namespace WebVanChuyen.Models
{
    public class ShipmentHistory
    {
        public int Id { get; set; }

        // Khóa ngoại tới Shipment
        public int ShipmentId { get; set; }
        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }

        public ShipmentStatus Status { get; set; } // Trạng thái tại thời điểm đó
        public string Location { get; set; } // Vị trí
        public DateTime Timestamp { get; set; }
    }
}
