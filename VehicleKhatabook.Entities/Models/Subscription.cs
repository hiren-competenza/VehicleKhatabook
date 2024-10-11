using Bonobo.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class Subscription : EntityBase
    {
        [Key]
        public Guid SubscriptionID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        [Required]
        [MaxLength(20)]
        public string SubscriptionType { get; set; } = "Free";

        public DateTime SubscriptionStartDate { get; set; } = DateTime.Now;
        public DateTime SubscriptionEndDate { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
