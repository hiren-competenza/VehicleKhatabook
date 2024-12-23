using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IIncomeService
    {
        Task<UserIncome> AddIncomeAsync(IncomeDTO incomeDTO);
        Task<UserIncome> UpdateIncomeAsync(IncomeDTO incomeDTO, int incomeExpenseId);
        Task<ApiResponse<UserIncome>> GetIncomeDetailsAsync(int id);
        //Task<ApiResponse<Income>> UpdateIncomeAsync(int id, IncomeDTO incomeDTO);
        //Task<ApiResponse<bool>> DeleteIncomeAsync(int id);
        //Task<ApiResponse<List<Income>>> GetAllIncomesAsync();
        Task<List<UserIncome>> GetIncomeAsync(Guid vehicleId, DateTime fromDate, DateTime toDate);
        Task<List<UserIncome>> GetIncomeAsync(Guid vehicleId);
        Task<List<UserIncome>> GetIncomebyUserAsync(Guid userId);
        Task<bool> DeleteIncomeAsync(int incomeExpenseId);
    }
}
