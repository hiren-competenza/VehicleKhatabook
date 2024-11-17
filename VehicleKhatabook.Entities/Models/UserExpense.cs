using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class UserExpense : EntityBase
    {
        [Key]
        public int ExpenseID { get; set; }

        [Required]
        public int ExpenseCategoryID { get; set; }

        public decimal ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? ExpenseDescription { get; set; }
        //public Guid UserID { get; set; }
        public Guid ExpenseVehicleId { get; set; }
        public bool? IsActive { get; set; }
        [ForeignKey("ExpenseCategoryID")]
        public ExpenseCategory ExpenseCategory { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
