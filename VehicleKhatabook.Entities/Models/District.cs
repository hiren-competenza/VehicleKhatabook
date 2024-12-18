using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    [Table("District")]
    public class District : EntityBase
    {
        [Key, Required]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public int StateId { get; set; }
        public State State { get; set; }
        [Required]
        public string DistrictName { get; set; } = null!;       

        [Required]
        public bool? IsActive { get; set; }
       
    }
}
