using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IEmailService emailService, IOTPService otpService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _otpService = otpService;
            _configuration = configuration;
        }

        public string GenerateToken(UserDetailsDTO userDetailsDTO)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userDetailsDTO.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userDetailsDTO.Email),
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
