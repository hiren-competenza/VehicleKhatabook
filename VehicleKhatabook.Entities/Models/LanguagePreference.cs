using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class LanguagePreference
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(10)]
        public string LanguageCode { get; set; } = "en";

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedOn { get; set; }
    }
}
