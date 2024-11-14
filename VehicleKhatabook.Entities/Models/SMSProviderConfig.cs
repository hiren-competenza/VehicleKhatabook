using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class SMSProviderConfig 
    {
        [Key]
        public int ProviderID { get; set; }

        [Required]
        [MaxLength(100)]
        public string? ProviderName { get; set; }

        [Required]
        [MaxLength(500)]
        public string? APIUrl { get; set; }

        [Required]
        [MaxLength(255)]
        public string? AuthKey { get; set; }

        [MaxLength(100)]
        public string? ClientID { get; set; }

        [Required]
        [MaxLength(50)]
        public string? SenderID { get; set; }

        [Required]
        public int? Timeout { get; set; } = 30;

        [Required]
        public bool? IsActive { get; set; } = true;

    }

}
