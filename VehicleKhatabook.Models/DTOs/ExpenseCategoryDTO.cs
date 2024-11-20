using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Models.DTOs
{
    public class ExpenseCategoryDTO
    {
        // public int ExpenseCategoryID { get; set; }
        public string Name { get; set; }
        public int? RoleId { get; set; }
        public string? Description { get; set; }
        public string? ExpenseCategoryLanguageJson { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ExpenseCategoryID { get; set; }
        public int ModifiedBy { get; set; }

    }
}
