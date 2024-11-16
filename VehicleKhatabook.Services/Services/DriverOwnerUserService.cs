using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class DriverOwnerUserService : IDriverOwnerUserService
    {
        private readonly IDriverOwnerUserRepository _repository;

        public DriverOwnerUserService(IDriverOwnerUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DriverOwnerUser>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<DriverOwnerUser>> GetByUserAsync(Guid userId)
        {
            // User ID can be fetched from claims in the endpoint level
            return await _repository.GetByUserAsync(userId); // Replace Guid.Empty with the actual UserID
        }

        public async Task<DriverOwnerUser> AddAsync(DriverOwnerUserDTO driverOwnerUserDTO, Guid userId)
        {
            return await _repository.AddAsync(driverOwnerUserDTO, userId);
        }

        public async Task<DriverOwnerUser> UpdateAsync(Guid id, DriverOwnerUserDTO driverOwnerUserDTO, Guid userId)
        {
            return await _repository.UpdateAsync(id, driverOwnerUserDTO, userId);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            await _repository.DeleteAsync(id, userId);
        }
    }


}
