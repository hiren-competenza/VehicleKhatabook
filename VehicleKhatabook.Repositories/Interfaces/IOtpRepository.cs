using VehicleKhatabook.Entities.Models;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IOtpRepository
    {
        Task SaveOtpAsync(OtpRequest otpRequest);
        Task<OtpRequest> GetOtpByUserIdAndCodeAsync(Guid userId, string otpCode, string otpRequestId);
        Task<OtpRequest> GetOtpByMobileAndCodeAsync(string mobileNumber, string otpCode, string otpRequestId);
        Task UpdateOtpAsync(OtpRequest otpRequest);
        Task SendOtpAsync(string mobileNumber, string otp, string SmsPurpose, string app_signature);
    }
}
