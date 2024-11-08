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
            new Claim(ClaimTypes.NameIdentifier, userDetailsDTO.UserId.ToString()),
            //new Claim(JwtRegisteredClaimNames.Email, userDetailsDTO.Email),
            new Claim("role", userDetailsDTO.RoleName.ToLower().ToString()),
            new Claim("firstname", userDetailsDTO.FirstName),
            new Claim("lastname", userDetailsDTO.LastName),
            new Claim(ClaimTypes.MobilePhone,userDetailsDTO.MobileNumber.Trim())
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
        public async Task<(bool Success, OtpRequest Otp)> SendOTPforRegisteredUser(string mobileNumber)
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

                return (true, otpRequest);
            }

            return (false, new OtpRequest());
        }

        public async Task<(bool Success, OtpRequest Otp)> SendOTPforAnonymousUser(string mobileNumber)
        {

            var otp = GenerateOTP();
            OtpRequest otpRequest = new OtpRequest
            {
                OtpCode = otp,
                MobileNumber = mobileNumber,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)
            };

            await _otpRepository.SaveOtpAsync(otpRequest);
            await _otpRepository.SendOtpAsync(mobileNumber, otp);

            return (true, otpRequest);
        }

        public async Task<bool> ResetMpinAsync(ResetMpinDTO resetMpinDTO, string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));
            if (user != null && BCrypt.Net.BCrypt.Verify(resetMpinDTO.CurrentMPin, user.mPIN))
            {
                user.mPIN = resetMpinDTO.NewMpin;
                await _userRepository.UpdateUserAsync(user);
                return true;
            }
            return false;
        }

        public async Task<bool> VerifyOtpbyUserIdAsync(Guid userId, string otpCode)
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

        public async Task<bool> VerifyOtpbyMobilePhoneAsync(string mobileNumber, string otpCode)
        {
            var otpRequest = await _otpRepository.GetOtpByMobileAndCodeAsync(mobileNumber, otpCode);
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
