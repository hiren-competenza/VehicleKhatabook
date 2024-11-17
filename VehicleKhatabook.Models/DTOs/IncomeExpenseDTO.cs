using System.Text.Json.Serialization;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Models.DTOs
{
    public class IncomeExpenseDTO
    {
        public int CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public Guid VehicleId { get; set; }
        [JsonConverter(typeof(NullableStringConverter))]
        public string CreatedBy { get; set; }
        [JsonConverter(typeof(NullableStringConverter))]

        public string? ModifiedBy { get; set; }
        public string TransactionType { get; set; }
    }
}
