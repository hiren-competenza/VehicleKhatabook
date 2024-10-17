using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class FuelTracking : EntityBase
    {
        [Key]
        public Guid FuelTrackingID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid VehicleID { get; set; }

        [Required]
        public Guid DriverID { get; set; }

        public decimal StartMeterReading { get; set; }
        public decimal EndMeterReading { get; set; }
        public decimal StartFuelLevel { get; set; }
        public decimal EndFuelLevel { get; set; }
        public decimal FuelAdded { get; set; }
        public decimal Mileage { get; set; }

        public DateTime TripStartDate { get; set; }
        public DateTime TripEndDate { get; set; }

        [ForeignKey("VehicleID")]
        public Vehicle Vehicle { get; set; }

        [ForeignKey("DriverID")]
        public User Driver { get; set; }
    }
}
