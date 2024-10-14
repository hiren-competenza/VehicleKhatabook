using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;

        public DriverService(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<ApiResponse<User>> AddDriverAsync(UserDTO driverDTO)
        {
            return await _driverRepository.AddDriverAsync(driverDTO);
        }

        public async Task<User?> GetDriverByIdAsync(Guid id)
        {
            return await _driverRepository.GetDriverByIdAsync(id);
        }

        public async Task<ApiResponse<User>> UpdateDriverAsync(Guid id, UserDTO driverDTO)
        {
            return await _driverRepository.UpdateDriverAsync(id, driverDTO);
        }

        public async Task<ApiResponse<bool>> DeleteDriverAsync(Guid id)
        {
            return await _driverRepository.DeleteDriverAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllDriversAsync()
        {
            return await _driverRepository.GetAllDriversAsync();
        }
    }
}
