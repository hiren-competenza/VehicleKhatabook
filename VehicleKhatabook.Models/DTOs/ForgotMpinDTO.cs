namespace VehicleKhatabook.Models.DTOs
{
    public class ForgotMpinDTO
    {
        public string MobileNumber { get; set; }
        public string? SmsPurpose { get; set; }
        public string? app_signature { get; set; }
    }
}
