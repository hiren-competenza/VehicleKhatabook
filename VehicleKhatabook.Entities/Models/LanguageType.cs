using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class LanguageType : EntityBase
    {
        [Key]
        public int LanguageTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string LanguageName { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }

}
