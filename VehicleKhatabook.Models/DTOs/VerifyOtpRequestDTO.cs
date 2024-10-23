namespace VehicleKhatabook.Models.DTOs
{
    public class VerifyOtpRequestDTO
    {
        public int UserId { get; set; }
        public string OtpCode { get; set; }
    }
}
