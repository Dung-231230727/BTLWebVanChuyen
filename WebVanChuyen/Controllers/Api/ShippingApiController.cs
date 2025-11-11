using WebVanChuyen.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BTLWebVanChuyen.Controllers.Api
{
    [Route("api/shipping")] // Đặt route cơ sở
    [ApiController]
    public class ShippingApiController : ControllerBase
    {
        private readonly IPricingService _pricingService;

        public ShippingApiController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        // 1. GET /api/shipping/calculate
        [HttpGet("calculate")]
        public async Task<IActionResult> CalculateFee(decimal weightKg, int fromZoneId, int toZoneId)
        {
            if (weightKg <= 0 || fromZoneId <= 0 || toZoneId <= 0)
            {
                return BadRequest(new { message = "Thông tin không hợp lệ." });
            }

            try
            {
                // Gọi IPricingService
                var fee = await _pricingService.CalculateFeeAsync(weightKg, fromZoneId, toZoneId);
                // Trả về JSON
                return Ok(new { calculatedFee = fee });
            }
            catch (InvalidOperationException ex)
            {
                // Trả về lỗi HTTP 400
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ." });
            }
        }
    }
}