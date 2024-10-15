using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IOTPService _otpService;

        public AuthService(IUserRepository userRepository, IEmailService emailService, IOTPService otpService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _otpService = otpService;
        }

        public async Task<User> AuthenticateUser(UserLoginDTO userLoginDTO)
        {
            var user = await _userRepository.GetUserByMobileNumberAsync(userLoginDTO.MobileNumber);

            if (user == null || user.mPIN != userLoginDTO.mPIN)
            {
                throw new UnauthorizedAccessException("Invalid mobile number or password.");
            }
            return user;
        }
        public async Task<bool> SendForgotMpinEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null)
            {
                var otp = _otpService.GenerateOTP();
                await _otpService.SaveOTPForUser(user.UserID, otp);
                await _emailService.SendOtpEmailAsync(email, otp);
                return true;
            }
            return false;
        }
            
        public async Task<bool> ResetMpinAsync(ResetMpinDTO resetMpinDTO)
        {
            var user = await _userRepository.GetUserByIdAsync(resetMpinDTO.UserId);
            if (user != null && _otpService.ValidateOTP(resetMpinDTO.UserId, resetMpinDTO.OTP))
            {   
                user.mPIN = resetMpinDTO.NewMpin;
                await _userRepository.UpdateUserAsync(resetMpinDTO.UserId, user);
                return true;
            }
            return false;
        }

        private string HashMpin(string mpin)
        {
            return BCrypt.Net.BCrypt.HashPassword(mpin);
        }
    }
}
