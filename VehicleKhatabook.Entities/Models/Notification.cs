using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class Notification : EntityBase
    {
        [Key]
        public Guid NotificationID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        [MaxLength(255)]
        public string Message { get; set; }

        public DateTime NotificationDate { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
