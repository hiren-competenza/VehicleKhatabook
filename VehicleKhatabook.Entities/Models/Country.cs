using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    [Table("Country")]
    public class Country
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, MaxLength(80)]
        public string Name { get; set; } = null!;

        [MaxLength(3)]
        public string? Code { get; set; }

        public int? DialCode { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }
    }
}
