using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class VechileType : EntityBase
    {
        [Key]
        public int VehicleTypeId { get; set; }
        [Required]
        [MaxLength(100)]
        public string TypeName { get; set; }

        public string? Description { get; set; }
        public string? VehicleTypeLanguageJson { get; set; }

        public bool? IsActive { get; set; }
    }
}
