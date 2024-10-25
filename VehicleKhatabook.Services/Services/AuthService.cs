using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IOtpRepository _otpRepository;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IOtpRepository otpRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _otpRepository = otpRepository;
        }

        public string GenerateToken(UserDetailsDTO userDetailsDTO)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userDetailsDTO.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userDetailsDTO.Email),
            new Claim("role", userDetailsDTO.RoleName.ToLower().ToString()),
            new Claim("firstname", userDetailsDTO.FirstName),
            new Claim("lastname", userDetailsDTO.LastName),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenExpiryMinutes = int.Parse(_configuration["Jwt:TokenExpiryMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenExpiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserDetailsDTO> AuthenticateUser(UserLoginDTO userLoginDTO)
        {
            return await _userRepository.AuthenticateUser(userLoginDTO);
        }
        public async Task<bool> SendForgotMpinAsync(string mobileNumber)
        {
            var user = await _userRepository.GetUserByMobileNumberAsync(mobileNumber);
            if (user != null)
            {
                var otp = GenerateOTP();
                OtpRequest otpRequest = new OtpRequest
                {
                    UserID = user.UserID,
                    OtpCode = otp,
                    MobileNumber = mobileNumber,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(10)
                };
                await _otpRepository.SaveOtpAsync(otpRequest);
                await _otpRepository.SendOtpAsync(mobileNumber, otp);
                return true;
            }
            return false;
        }

        public async Task<bool> ResetMpinAsync(ResetMpinDTO resetMpinDTO)
        {
            var user = await _userRepository.GetUserByIdAsync(resetMpinDTO.UserId);
            bool isValid = await VerifyOtp(resetMpinDTO.UserId, resetMpinDTO.OTP);
            if (user != null && isValid)
            {   
                user.mPIN = resetMpinDTO.NewMpin;
                await _userRepository.UpdateUserAsync(resetMpinDTO.UserId, user);
                return true;
            }
            return false;
        }
        
        public async Task<bool> VerifyOtp(Guid userId, string otpCode)
        {
            var otpRequest = await _otpRepository.GetOtpByUserIdAndCodeAsync(userId, otpCode);
            if (otpRequest == null || otpRequest.ExpirationTime < DateTime.UtcNow || otpRequest.IsVerified)
            {
                return false;
            }

            otpRequest.IsVerified = true;
            await _otpRepository.UpdateOtpAsync(otpRequest);
            return true;
        }

        public string GenerateOTP()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
