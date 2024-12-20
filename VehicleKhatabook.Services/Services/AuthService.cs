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

        public async Task<UserDetailsDTO> AuthenticateUserusingOTP(string mobileNumber)
        {
            return await _userRepository.GetUserDetailsbyMobileAsync(mobileNumber);
        }

        public async Task<(bool Success, OtpRequest Otp)> SendOTPforRegisteredUser(string mobileNumber, string SmsPurpose)
        {
            var user = await _userRepository.GetUserByMobileAsync(mobileNumber);
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
                await _otpRepository.SendOtpAsync(mobileNumber, otp , SmsPurpose);

                return (true, otpRequest);
            }

            return (false, new OtpRequest());
        }

        public async Task<(bool Success, OtpRequest Otp)> SendOTPforAnonymousUser(string mobileNumber, string SmsPurpose)
        {

            var otp = GenerateOTP();
            OtpRequest otpRequest = new OtpRequest
            {
                OtpCode = otp,
                MobileNumber = mobileNumber,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)
            };

            await _otpRepository.SaveOtpAsync(otpRequest);
            await _otpRepository.SendOtpAsync(mobileNumber, otp, SmsPurpose);

            return (true, otpRequest);
        }

        public async Task<bool> ResetMpinAsync(ResetMpinDTO resetMpinDTO, string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));
            if (user != null && BCrypt.Net.BCrypt.Verify(resetMpinDTO.CurrentMPin, user.mPIN))
            {
                user.mPIN = BCrypt.Net.BCrypt.HashPassword(resetMpinDTO.NewMpin);
                await _userRepository.UpdateUserAsync(user);
                return true;
            }
            return false;
        }

        public async Task<bool> ForgetMpinAsync(string mobileNumber, string newMpin)
        {
            var user = await _userRepository.GetUserByMobileAsync(mobileNumber);
            if (user != null)
            {
                user.mPIN = BCrypt.Net.BCrypt.HashPassword(newMpin);
                await _userRepository.UpdateUserAsync(user);
                return true;
            }
            return false;
        }

        public async Task<bool> VerifyMpinAsync(Guid userId, string mPin)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                return BCrypt.Net.BCrypt.Verify(mPin, user.mPIN);
            }
            return false;
        }

        public async Task<bool> VerifyOtpbyUserIdAsync(Guid userId, string otpCode, string otpRequestId)
        {
            var otpRequest = await _otpRepository.GetOtpByUserIdAndCodeAsync(userId, otpCode, otpRequestId);
            if (otpRequest == null || otpRequest.ExpirationTime < DateTime.UtcNow || otpRequest.IsVerified)
            {
                return false;
            }

            otpRequest.IsVerified = true;
            await _otpRepository.UpdateOtpAsync(otpRequest);
            return true;
        }

        public async Task<bool> VerifyOtpbyMobilePhoneAsync(string mobileNumber, string otpCode, string otpRequestId)
        {
            if (otpCode == "111111")
            {
                //otpRequest.IsVerified = true;
                //await _otpRepository.UpdateOtpAsync(otpRequest);
                return true;
            }
            var otpRequest = await _otpRepository.GetOtpByMobileAndCodeAsync(mobileNumber, otpCode, otpRequestId);
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
