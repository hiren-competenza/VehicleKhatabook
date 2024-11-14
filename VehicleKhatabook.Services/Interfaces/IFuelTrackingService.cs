using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IFuelTrackingService
    {
        Task<FuelTrackingDTO> AddFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO);
        Task<FuelTrackingDTO> UpdateFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO);
        Task<FuelTrackingDTO> GetFuelTrackingAsync();
    }

}
