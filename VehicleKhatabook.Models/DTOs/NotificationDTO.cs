namespace VehicleKhatabook.Models.DTOs
{
    public class NotificationDTO
    {
        public Guid NotificationID { get; set; }
        public Guid UserID { get; set; }
        public string? Message { get; set; } // Allow null values
        public DateTime? NotificationDate { get; set; } // Allow null values
        public bool IsRead { get; set; }
    }
}
