namespace VehicleKhatabook.Models.DTOs
{
    public class LanguagePreferenceDTO
    {
        public Guid UserId { get; set; }
        public string LanguageCode { get; set; } = "en";
    }
}
