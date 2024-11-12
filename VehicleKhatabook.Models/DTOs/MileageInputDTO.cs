using System.Text.Json.Serialization;

namespace VehicleKhatabook.Models.DTOs
{
    public class MileageInputDTO
    {

        [JsonPropertyName("startVehicleMeterReading")]
        public int StartVehicleMeterReading { get; set; }

        [JsonPropertyName("endVehicleMeterReading")]
        public int EndVehicleMeterReading { get; set; }

        [JsonPropertyName("startFuelLevelInLiters")]
        public double StartFuelLevelInLiters { get; set; }

        [JsonPropertyName("endFuelLevelInLiters")]
        public double EndFuelLevelInLiters { get; set; }

        [JsonPropertyName("fuelAddedInLiters")]
        public List<double> FuelAddedInLiters { get; set; }

    }
}
