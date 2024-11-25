using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace VehicleKhatabook.Entities.Models
{
    public class FuelTracking : EntityBase
    {
        [Key]
        public int Id { get; set; }

        [Column("StartVehicleMeterReading", TypeName = "int")]
        public int StartVehicleMeterReading { get; set; }

        [Column("EndVehicleMeterReading", TypeName = "int")]
        public int? EndVehicleMeterReading { get; set; }

        [Column("StartFuelLevelInLiters", TypeName = "decimal(18, 0)")]
        public decimal StartFuelLevelInLiters { get; set; }

        [Column("EndFuelLevelInLiters", TypeName = "decimal(18, 0)")]
        public decimal? EndFuelLevelInLiters { get; set; }

        [Column("FuelAddedInLitersJson", TypeName = "nvarchar(max)")]
        public string? FuelAddedInLitersJson { get; set; }

        // This property is not mapped to the database, used for ease of access to FuelAddedInLiters as a list.
        [NotMapped]
        public List<decimal> FuelAddedInLiters
        {
            get => string.IsNullOrEmpty(FuelAddedInLitersJson)
                   ? new List<decimal>()
                   : JsonSerializer.Deserialize<List<decimal>>(FuelAddedInLitersJson);
            set => FuelAddedInLitersJson = JsonSerializer.Serialize(value);
        }

        [Column("UserId", TypeName = "uniqueidentifier")]
        public Guid UserId { get; set; }

        // Optional: Add a navigation property to access User details (this will require a User class)
        public virtual User User { get; set; }
    }


}
