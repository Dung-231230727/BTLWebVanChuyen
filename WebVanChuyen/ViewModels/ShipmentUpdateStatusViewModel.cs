using WebVanChuyen.Models;
using System.ComponentModel.DataAnnotations;

namespace WebVanChuyen.ViewModels
{
    public class ShipmentUpdateStatusViewModel
    {
        public string TrackingNumber { get; set; }

        [Required]
        public ShipmentStatus NewStatus { get; set; }

        [Required]
        public string Location { get; set; }
    }
}