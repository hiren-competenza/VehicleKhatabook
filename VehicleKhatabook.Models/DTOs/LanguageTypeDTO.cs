namespace VehicleKhatabook.Models.DTOs
{
    public class LanguageTypeDTO
    {
        public int LanguageTypeId { get; set; }
        public string LanguageName { get; set; }
        public string Description { get; set; }
        public string? Locale { get; set; }
        public bool IsActive { get; set; }
    }
}
