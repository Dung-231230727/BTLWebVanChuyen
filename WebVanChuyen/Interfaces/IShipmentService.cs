using WebVanChuyen.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebVanChuyen.Interfaces
{
    public interface IShipmentService
    {
        Task<List<Shipment>> GetAllShipmentsAsync();
        Task<Shipment> GetShipmentByIdAsync(int id);
        Task<Shipment> GetShipmentByTrackingNumberAsync(string trackingNumber);
        Task<Shipment> CreateShipmentAsync(Shipment newShipment);
        Task UpdateShipmentStatusAsync(string trackingNumber, ShipmentStatus newStatus, string location);
    }
}