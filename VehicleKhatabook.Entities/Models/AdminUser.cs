using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class AdminUser : EntityBase
    {
        [Key]
        public int AdminID { get; set; }

        [Required]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string? PasswordHash { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Role { get; set; }

        [Required]
        [MaxLength(255)]
        public string? SecurityQuestion { get; set; }
        [Required]
        [MaxLength(255)]
        public string? SecurityAnswerHash { get; set; }

        [Required]
        [MaxLength(15)]
        public string? MobileNumber { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime? LastLogin { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual AdminUser? CreatedByAdmin { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual AdminUser? ModifiedByAdmin { get; set; }
    }

}
