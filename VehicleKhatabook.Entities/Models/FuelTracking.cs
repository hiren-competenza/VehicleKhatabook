using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace VehicleKhatabook.Entities.Models
{
    public class FuelTracking : EntityBase
    {
        [Key]
        public int Id { get; set; }

        public int StartVehicleMeterReading { get; set; }   
        public int EndVehicleMeterReading { get; set; }
        public double StartFuelLevelInLiters { get; set; }
        public double EndFuelLevelInLiters { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string FuelAddedInLitersJson { get; set; }
        // This property is not mapped to the database, used for ease of access to FuelAddedInLiters as a list.
        [NotMapped]
        public List<double> FuelAddedInLiters
        {
            get => string.IsNullOrEmpty(FuelAddedInLitersJson)
                   ? new List<double>()
                   : JsonSerializer.Deserialize<List<double>>(FuelAddedInLitersJson);
            set => FuelAddedInLitersJson = JsonSerializer.Serialize(value);
        }
    }
}
