using Bonobo.Entities;
using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class User : EntityBase
    {
        [Key, Required]
        public Guid UserID { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [Required]
        [MaxLength(15)]
        public string? MobileNumber { get; set; }

        public string? mPIN { get; set; }
        public string? ReferCode { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Role { get; set; }
        public Guid IdentifierId { get; set; } = Guid.NewGuid();

        public bool IsPremiumUser { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Language { get; set; }

        public bool IsActive { get; set; }
    }
}
