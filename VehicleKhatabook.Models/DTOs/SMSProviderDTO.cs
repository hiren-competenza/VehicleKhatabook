namespace VehicleKhatabook.Models.DTOs
{
    public class SMSProviderDTO
    {
        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string APIUrl { get; set; }
        public string AuthKey { get; set; }
        public string? ClientID { get; set; }
        public string SenderID { get; set; }
        public int? Timeout { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }

}
