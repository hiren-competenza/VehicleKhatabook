using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class ScreenContent : EntityBase
    {
        [Key]
        public int ScreenContentID { get; set; }

        [MaxLength(100)]
        public string? ScreenName { get; set; }

        [MaxLength(100)]
        public string? ContentKey { get; set; }

        [MaxLength(255)]
        public string? ContentValue { get; set; }

        [MaxLength(10)]
        public string? Language { get; set; }

        public bool IsActive { get; set; }
    }
}
