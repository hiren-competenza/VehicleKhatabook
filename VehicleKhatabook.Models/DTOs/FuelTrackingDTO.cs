namespace VehicleKhatabook.Models.DTOs
{
    public class FuelTrackingDTO
    {
        public Guid VehicleID { get; set; }
        public Guid DriverID { get; set; }
        public decimal StartMeterReading { get; set; }
        public decimal EndMeterReading { get; set; }
        public decimal StartFuelLevel { get; set; }
        public decimal EndFuelLevel { get; set; }
        public decimal FuelAdded { get; set; }
        public DateTime TripStartDate { get; set; }
        public DateTime TripEndDate { get; set; }
    }
}
