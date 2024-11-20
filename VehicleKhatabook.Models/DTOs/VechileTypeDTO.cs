using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Models.DTOs
{
    public class VechileTypeDTO
    {
        public string TypeName { get; set; }
        public string? Description { get; set; }
        public string? VehicleTypeLanguageJson { get; set; }
        public bool? IsActive { get; set; }
    }
}
