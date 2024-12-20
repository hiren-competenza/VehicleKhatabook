using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using static System.Net.WebRequestMethods;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDetailsDTO> AuthenticateUser(UserLoginDTO userLoginDTO);
        Task<UserDetailsDTO> AuthenticateUserusingOTP(string mobileNumber);
        Task<(bool Success, OtpRequest Otp)> SendOTPforRegisteredUser(string mobileNumber, string SmsPurpose);
        Task<(bool Success, OtpRequest Otp)> SendOTPforAnonymousUser(string mobileNumber, string SmsPurpose);
        Task<bool> ResetMpinAsync(ResetMpinDTO resetMpinDTO, string userId);
        Task<bool> ForgetMpinAsync(string mobileNumber, string newMpin);
        Task<bool> VerifyMpinAsync(Guid userId, string mPin);
        string GenerateToken(UserDetailsDTO userDetailsDTO);
        Task<bool> VerifyOtpbyUserIdAsync(Guid userId, string otpCode, string otpRequestId);
        Task<bool> VerifyOtpbyMobilePhoneAsync(string mobileNumber, string otpCode, string otpRequestId);
    }
}
