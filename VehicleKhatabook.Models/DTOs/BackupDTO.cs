namespace VehicleKhatabook.Models.DTOs
{
    public class BackupDTO
    {
        public Guid BackupID { get; set; }
        public Guid UserID { get; set; }
        public DateTime BackupDate { get; set; }
        public byte[] BackupData { get; set; } = Array.Empty<byte>();
    }
}
