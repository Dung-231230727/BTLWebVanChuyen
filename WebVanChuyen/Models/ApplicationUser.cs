using Microsoft.AspNetCore.Identity;

namespace WebVanChuyen.Models
{
    // Kế thừa IdentityUser và thêm FullName
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        // Navigation Property (một User có nhiều Shipment)
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}
