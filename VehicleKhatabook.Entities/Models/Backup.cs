using Bonobo.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class Backup : EntityBase
    {
        [Key]
        public Guid BackupID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        public DateTime BackupDate { get; set; }
        public byte[] BackupData { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }

}
