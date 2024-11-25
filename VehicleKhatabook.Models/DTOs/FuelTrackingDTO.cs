using System.Text.Json.Serialization;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Models.DTOs
{
    public class FuelTrackingDTO
    {
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("startVehicleMeterReading")]
        public int StartVehicleMeterReading { get; set; }

        [JsonPropertyName("endVehicleMeterReading")]
        public int? EndVehicleMeterReading { get; set; }

        [JsonPropertyName("startFuelLevelInLiters")]
        public decimal StartFuelLevelInLiters { get; set; }

        [JsonPropertyName("endFuelLevelInLiters")]
        public decimal? EndFuelLevelInLiters { get; set; }

        [JsonPropertyName("fuelAddedInLiters")]
        public List<decimal>? FuelAddedInLiters { get; set; }

    }
}
