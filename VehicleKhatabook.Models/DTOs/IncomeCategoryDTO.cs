using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Models.DTOs
{
    public class IncomeCategoryDTO
    {
        //public int? IncomeCategoryID { get; set; }
        public string Name { get; set; }
        public int? RoleId { get; set; }
        public string? Description { get; set; }
        public string? IncomeCategoryLanguageJson { get; set; }

        public bool? IsActive { get; set; }
    }
}
