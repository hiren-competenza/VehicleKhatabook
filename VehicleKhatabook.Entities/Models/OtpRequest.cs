using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class OtpRequest : EntityBase
    {
        [Key]
        public Guid OtpRequestId { get; set; } = Guid.NewGuid();

        public Guid UserID { get; set; }

        [Required]
        [MaxLength(6)]
        public string OtpCode { get; set; }

        [Required]
        [MaxLength(15)]
        public string MobileNumber { get; set; }

        [Required]
        public DateTime ExpirationTime { get; set; }

        public bool IsVerified { get; set; } = false;

        //public virtual User User { get; set; }
    }

}
