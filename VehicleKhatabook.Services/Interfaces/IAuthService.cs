using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDetailsDTO> AuthenticateUser(UserLoginDTO userLoginDTO);
        Task<(bool Success, string Otp)> SendForgotMpinAsync(string mobileNumber);
        Task<bool> ResetMpinAsync(ResetMpinDTO resetMpinDTO);
        string GenerateToken(UserDetailsDTO userDetailsDTO);
        Task<bool> VerifyOtpAsync(Guid userId, string otpCode);
    }
}
