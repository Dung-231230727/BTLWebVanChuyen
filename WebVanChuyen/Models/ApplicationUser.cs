using Microsoft.AspNetCore.Identity;

namespace WebVanChuyen.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        // Navigation Property
        public ICollection<Shipment> SentShipments { get; set; }
    }
}
