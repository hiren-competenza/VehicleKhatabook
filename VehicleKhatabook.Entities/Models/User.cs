using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class User : EntityBase
    {
        [Key]
        public Guid UserID { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [Required]
        [MaxLength(15)]
        public string MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? mPIN { get; set; }
        public string? ReferCode { get; set; }
        public string? UserReferCode { get; set; }
        [Required]
        public int UserTypeId { get; set; }
        [Required]
        [MaxLength(10)]
        public string? Role { get; set; }
        public Guid IdentifierId { get; set; } = Guid.NewGuid();

        public bool? IsPremiumUser { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }

        //public int LanguageTypeId { get; set; } 

        [ForeignKey("LanguageTypeId")] 
        public LanguageType? LanguageType { get; set; }
        public int? LanguageTypeId { get; set; }

        public bool? IsActive { get; set; }
        public virtual ICollection<SMSProviderConfig> CreatedSMSProviderConfigs { get; set; }
        public virtual ICollection<SMSProviderConfig> ModifiedSMSProviderConfigs { get; set; }
    }
}
