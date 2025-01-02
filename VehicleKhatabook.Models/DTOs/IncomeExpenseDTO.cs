using System.Text.Json.Serialization;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Models.DTOs
{
    public interface IHasVehicleTransactionDates
    {
        DateTime? TransactionDate { get; set; }
    }
    public class IncomeExpenseDTO
    {
        public int CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public Guid VehicleId { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public string TransactionType { get; set; }

        // Nested objects for category and vehicle
        public IncomeCategoryDTO IncomeCategory { get; set; }
        public VehicleDTO Vehicle { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
