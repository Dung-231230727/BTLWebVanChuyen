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
}
