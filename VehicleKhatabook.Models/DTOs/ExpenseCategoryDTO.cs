using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Models.DTOs
{
    public class ExpenseCategoryDTO
    {
       // public int ExpenseCategoryID { get; set; }
        public string Name { get; set; }
        public int? RoleId { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
