using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Models.DTOs
{
    public interface IHasTransactionDates
    {
        DateTime? TransactionDate { get; set; }
    }

    public class OwnerIncomeExpenseDTO : IHasTransactionDates
    {
        public Guid? Id { get; set; }
        public Guid DriverOwnerUserId { get; set; }
        public DateTime Date { get; set; }
        public string TransactionType { get; set; } = "";
        public decimal? Amount { get; set; }
        public string? Note { get; set; }
        public DateTime? TransactionDate { get; set; }
    }

}
