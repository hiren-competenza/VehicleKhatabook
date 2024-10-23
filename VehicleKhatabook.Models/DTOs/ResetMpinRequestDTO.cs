namespace VehicleKhatabook.Models.DTOs
{
    public class ResetMpinRequestDTO
    {
        public int UserId { get; set; }
        public string NewMpin { get; set; }
        public string OtpCode { get; set; }
    }
}
