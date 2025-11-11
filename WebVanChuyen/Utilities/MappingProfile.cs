using AutoMapper;
using WebVanChuyen.Models;
using WebVanChuyen.ViewModels;

namespace WebVanChuyen.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 2. Ánh xạ ShipmentCreateViewModel sang Shipment Entity
            CreateMap<ShipmentCreateViewModel, Shipment>();
        }
    }
}
