namespace VehicleKhatabook.Models.DTOs
{
    public class NotificationDTO
    {
        public Guid NotificationID { get; set; }
        public Guid UserID { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime NotificationDate { get; set; }
        public bool IsRead { get; set; }
    }
}
