using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> AuthenticateUser(UserLoginDTO userLoginDTO);
        Task<bool> SendForgotMpinEmailAsync(string email);
        Task<bool> ResetMpinAsync(ResetMpinDTO resetMpinDTO);
    }
}
