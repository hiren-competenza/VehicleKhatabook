using Bonobo.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class Expense : EntityBase
    {
        [Key]
        public int ExpenseID { get; set; }

        [Required]
        public int ExpenseCategoryID { get; set; }

        public decimal ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }

        public string ExpenseDescription { get; set; }
        public Guid DriverID { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ExpenseCategoryID")]
        public ExpenseCategory ExpenseCategory { get; set; }

        [ForeignKey("DriverID")]
        public User Driver { get; set; }
    }
}