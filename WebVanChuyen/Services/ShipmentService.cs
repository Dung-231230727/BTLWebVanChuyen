using WebVanChuyen.Data;
using WebVanChuyen.Interfaces;
using WebVanChuyen.Models;
using Microsoft.EntityFrameworkCore;
using System; // Thêm
using System.Collections.Generic;
using System.Linq; // Thêm
using System.Threading.Tasks;

namespace WebVanChuyen.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly AppDbContext _context;

        // Tiêm AppDbContext
        public ShipmentService(AppDbContext context)
        {
            _context = context;
        }

        // Triển khai GetAllShipmentsAsync
        public async Task<List<Shipment>> GetAllShipmentsAsync()
        {
            return await _context.Shipments.ToListAsync();
        }

        // Hoàn thiện logic tạo đơn hàng
        public async Task<Shipment> CreateShipmentAsync(Shipment newShipment)
        {
            // Sử dụng Transaction để đảm bảo toàn vẹn
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Gán TrackingNumber (10 ký tự ngẫu nhiên)
                newShipment.TrackingNumber = GenerateRandomTrackingNumber(10);
                // 2. Gán CurrentStatus và CreatedDate
                newShipment.CurrentStatus = ShipmentStatus.Pending;
                newShipment.CreatedDate = DateTime.UtcNow;

                // 3. Lưu Shipment vào DB
                _context.Shipments.Add(newShipment);
                await _context.SaveChangesAsync(); // Lưu để lấy ShipmentId

                // 4. Tạo bản ghi ShipmentHistory tương ứng
                var history = new ShipmentHistory
                {
                    ShipmentId = newShipment.Id,
                    Status = ShipmentStatus.Pending,
                    Location = "Bưu cục gửi",
                    Timestamp = newShipment.CreatedDate
                };
                _context.ShipmentHistories.Add(history);
                await _context.SaveChangesAsync();

                // Nếu mọi thứ thành công, commit transaction
                await transaction.CommitAsync();

                return newShipment;
            }
            catch (Exception)
            {
                // Nếu có lỗi, rollback
                await transaction.RollbackAsync();
                throw; // Ném lỗi ra ngoài
            }
        }

        // Hàm trợ giúp tạo TrackingNumber
        private string GenerateRandomTrackingNumber(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // ... (Các hàm khác sẽ hoàn thiện ở Bước 6 & 7)
        public Task<Shipment> GetShipmentByIdAsync(int id)
        {
            return _context.Shipments.FindAsync(id).AsTask();
        }

        // 2. Cập nhật logic tra cứu
        public async Task<Shipment> GetShipmentByTrackingNumberAsync(string trackingNumber)
        {
            return await _context.Shipments
                // Sử dụng EF Core .Include()
                .Include(s => s.Sender) // Tải kèm ApplicationUser (Sender)
                .Include(s => s.History
                    .OrderBy(h => h.Timestamp)) // Tải kèm ShipmentHistory và sắp xếp
                .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);
        }

        // 2. Hoàn thiện logic cập nhật trạng thái
        public async Task UpdateShipmentStatusAsync(string trackingNumber, ShipmentStatus newStatus, string location)
        {
            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);

            if (shipment == null)
            {
                throw new InvalidOperationException("Không tìm thấy mã vận đơn.");
            }

            // Sử dụng Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Cập nhật trạng thái của Shipment
                shipment.CurrentStatus = newStatus;

                // 2. Tạo bản ghi lịch sử mới
                var history = new ShipmentHistory
                {
                    ShipmentId = shipment.Id,
                    Status = newStatus,
                    Location = location,
                    Timestamp = DateTime.UtcNow
                };
                _context.ShipmentHistories.Add(history);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
