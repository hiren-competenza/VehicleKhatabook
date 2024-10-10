using Bonobo.Entities;
using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class ExpenseCategory : EntityBase
    {
        [Key]
        public int ExpenseCategoryID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
