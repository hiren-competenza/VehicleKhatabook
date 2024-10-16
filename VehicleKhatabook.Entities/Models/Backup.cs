using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class Backup : EntityBase
    {
        [Key]
        public Guid BackupID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserID { get; set; }

        public DateTime BackupDate { get; set; } = DateTime.UtcNow;
        public byte[] BackupData { get; set; } =  Array.Empty<byte>();

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
