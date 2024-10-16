using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class Vehicle : EntityBase
    {
        [Key]
        public Guid VehicleID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserID { get; set; }

        [MaxLength(50)]
        public string VehicleType { get; set; }

        [MaxLength(50)]
        public string RegistrationNumber { get; set; }

        public string NickName { get; set; }

        public DateTime? InsuranceExpiry { get; set; }
        public DateTime? PollutionExpiry { get; set; }
        public DateTime? FitnessExpiry { get; set; }
        public DateTime? RoadTaxExpiry { get; set; }
        public DateTime? RCPermitExpiry { get; set; }
        public DateTime? NationalPermitExpiry { get; set; }
        public string ChassisNumber { get; set; }
        public string EngineNumber { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
