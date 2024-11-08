using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDetailsDTO> AuthenticateUser(UserLoginDTO userLoginDTO);
        Task<(bool Success, OtpRequest Otp)> SendOTPforRegisteredUser(string mobileNumber);
        Task<(bool Success, OtpRequest Otp)> SendOTPforAnonymousUser(string mobileNumber);
        Task<bool> ResetMpinAsync(ResetMpinDTO resetMpinDTO, string userId);
        string GenerateToken(UserDetailsDTO userDetailsDTO);
        Task<bool> VerifyOtpbyUserIdAsync(Guid userId, string otpCode);
        Task<bool> VerifyOtpbyMobilePhoneAsync(string mobileNumber, string otpCode);
    }
}
