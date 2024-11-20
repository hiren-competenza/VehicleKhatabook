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
        [Required]
        public int? RoleId { get; set; }

        public string? Description { get; set; }
        public string? ExpenseCategoryLanguageJson { get; set; }

        public bool? IsActive { get; set; }
    }
}
