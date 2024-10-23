using VehicleKhatabook.Entities.Models;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IOtpRepository
    {
        Task SaveOtpAsync(OtpRequest otpRequest);
        Task<OtpRequest> GetOtpByUserIdAndCodeAsync(Guid userId, string otpCode);
        Task UpdateOtpAsync(OtpRequest otpRequest);
        Task SendOtpAsync(string mobileNumber, string otp);
    }
}
