namespace WebVanChuyen.Models
{
    public class ShipmentHistory
    {
        public int Id { get; set; }

        // Foreign Key
        public int ShipmentId { get; set; }

        public ShipmentStatus Status { get; set; }
        public string Location { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Navigation Property
        public Shipment Shipment { get; set; }
    }
}
