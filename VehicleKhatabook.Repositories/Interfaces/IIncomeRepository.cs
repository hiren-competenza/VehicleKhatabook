using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IIncomeRepository
    {
        Task<UserIncome> AddIncomeAsync(IncomeDTO incomeDTO);
        Task<ApiResponse<UserIncome>> GetIncomeDetailsAsync(int id);
        //Task<ApiResponse<Income>> UpdateIncomeAsync(int id, IncomeDTO incomeDTO);
        //Task<ApiResponse<bool>> DeleteIncomeAsync(int id);
        //Task<ApiResponse<List<Income>>> GetAllIncomesAsync();
        Task<List<UserIncome>> GetIncomeAsync(Guid vehicleId, DateTime fromDate, DateTime toDate);
        Task<List<IncomeExpenseDTO>> GetIncomeAsync(Guid vehicleId);
        Task<List<IncomeExpenseDTO>> GetIncomebyUserAsync(Guid userId);
        Task<UserIncome>UpdateIncomeAsync(IncomeDTO incomeDTO, int incomeExpenseId);
        Task<bool> DeleteIncomeAsync(int incomeExpenseId);
        //Task<List<UserIncome>> GetIncomebyUserAsync(Guid userId, int languageTypeId);

    }
}
