using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}
