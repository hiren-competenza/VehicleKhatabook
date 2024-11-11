using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public OtpRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task SaveOtpAsync(OtpRequest otpRequest)
        {
            _context.OtpRequests.Add(otpRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<OtpRequest> GetOtpByUserIdAndCodeAsync(Guid userId, string otpCode, string otpRequestId)
        {
            return await _context.OtpRequests
                .FirstOrDefaultAsync(o => o.UserID == userId && o.OtpCode == otpCode && o.OtpRequestId == new Guid(otpRequestId));
        }

        public async Task<OtpRequest> GetOtpByMobileAndCodeAsync(string mobileNumber, string otpCode, string otpRequestId)
        {
            return await _context.OtpRequests
                .FirstOrDefaultAsync(o => o.MobileNumber == mobileNumber && o.OtpCode == otpCode && o.OtpRequestId == new Guid(otpRequestId));
        }

        public async Task UpdateOtpAsync(OtpRequest otpRequest)
        {
            _context.OtpRequests.Update(otpRequest);
            await _context.SaveChangesAsync();
        }
        public Task SendOtpAsync(string mobileNumber, string otp)
        {
            Console.WriteLine($"Sending OTP {otp} to {mobileNumber}");
            return Task.CompletedTask;
        }
    }
}
