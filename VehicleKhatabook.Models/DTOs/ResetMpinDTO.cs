namespace VehicleKhatabook.Models.DTOs
{
    public class ResetMpinDTO
    {
        public Guid UserId { get; set; }
        public string OTP { get; set; }
        public string NewMpin { get; set; }
    }

}
