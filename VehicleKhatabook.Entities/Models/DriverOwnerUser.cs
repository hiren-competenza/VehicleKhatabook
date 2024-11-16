using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class DriverOwnerUser : EntityBase
    {
        [Key]
        public Guid DriverOwnerUserId { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(15)]
        public string MobileNumber { get; set; }
        [Required]
        public string? UserType { get; set; }
        public bool? IsActive { get; set; }
        [ForeignKey("UserID")]
        public User user { get; set; }
    }
}
