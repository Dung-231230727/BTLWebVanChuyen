using AutoMapper;
using BTLWebVanChuyen.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Thêm
using WebVanChuyen.Interfaces;
using WebVanChuyen.Models;
using WebVanChuyen.ViewModels;

namespace BTLWebVanChuyen.Controllers
{
    [Authorize] // Yêu cầu đăng nhập
    public class ShipmentController : Controller
    {
        private readonly IShipmentService _shipmentService;
        private readonly IPricingService _pricingService;
        private readonly IMapper _mapper;

        // 1. Tiêm dịch vụ
        public ShipmentController(IShipmentService shipmentService, IPricingService pricingService, IMapper mapper)
        {
            _shipmentService = shipmentService;
            _pricingService = pricingService;
            _mapper = mapper;
        }

        // 1. Action Index (Dashboard)
        [HttpGet]
        [Authorize(Roles = "Admin,Staff")] // Chỉ Admin hoặc Staff
        public async Task<IActionResult> Index()
        {
            // 2. Gọi Service
            var shipments = await _shipmentService.GetAllShipmentsAsync();

            // 3. Truyền danh sách vào View
            return View(shipments);
        }

        // GET: /Shipment/Create
        [HttpGet]
        public IActionResult Create()
        {
            // (Cần thêm logic tải Zones cho Dropdown)
            return View();
        }

        // POST: /Shipment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipmentCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                // 3. Ánh xạ ViewModel sang Entity
                var shipment = _mapper.Map<Shipment>(viewModel);

                // 2. Lấy UserId hiện tại
                shipment.SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // 4. Gọi IPricingService để tính ShippingFee
                shipment.ShippingFee = await _pricingService.CalculateFeeAsync(
                    shipment.WeightKg,
                    viewModel.FromZoneId,
                    viewModel.ToZoneId);

                // 5. Gọi IShipmentService.CreateShipmentAsync
                var createdShipment = await _shipmentService.CreateShipmentAsync(shipment);

                // 6. Redirect về Action Track
                return RedirectToAction("Track", new { trackingNumber = createdShipment.TrackingNumber });
            }
            catch (InvalidOperationException ex) // Bắt lỗi nếu không tìm thấy Rule
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi tạo đơn hàng.");
                return View(viewModel);
            }
        }

        // 1. Action Track (GET)
        [AllowAnonymous] // Cho phép truy cập công khai
        [HttpGet]
        public async Task<IActionResult> Track(string trackingNumber)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber))
            {
                // Nếu không có trackingNumber, chỉ hiển thị View (cho form nhập)
                return View();
            }

            // 2. Gọi Service để lấy thông tin
            var shipment = await _shipmentService.GetShipmentByTrackingNumberAsync(trackingNumber);

            if (shipment == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy mã vận đơn.";
            }

            // 3. Truyền Model vào View
            return View(shipment);
        }

        // 1. Action UpdateStatus (GET)
        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStatus(int id) // Nhận Id (từ 7.1)
        {
            var shipment = await _shipmentService.GetShipmentByIdAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }

            // Chuẩn bị ViewModel
            var viewModel = new ShipmentUpdateStatusViewModel
            {
                TrackingNumber = shipment.TrackingNumber,
                NewStatus = shipment.CurrentStatus,
                Location = "" // Để trống cho nhân viên nhập
            };

            return View(viewModel);
        }

        // 2. Action UpdateStatus (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStatus(ShipmentUpdateStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // 3. Gọi Service
                await _shipmentService.UpdateShipmentStatusAsync(
                    model.TrackingNumber,
                    model.NewStatus,
                    model.Location);

                // Quay về Dashboard
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }
    }
}