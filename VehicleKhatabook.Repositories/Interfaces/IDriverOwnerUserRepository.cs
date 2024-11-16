using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IDriverOwnerUserRepository
    {
        Task<IEnumerable<DriverOwnerUser>> GetAllAsync();
        Task<IEnumerable<DriverOwnerUser>> GetByUserAsync(Guid userId);
        Task<DriverOwnerUser> AddAsync(DriverOwnerUserDTO driverOwnerUserDTO, Guid userId);
        Task<DriverOwnerUser> UpdateAsync(Guid id, DriverOwnerUserDTO driverOwnerUserDTO, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }


}
