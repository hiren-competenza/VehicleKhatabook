using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    [Table("State")]
    public class State : EntityBase
    {
        [Key, Required]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public int CountryId { get; set; }
        public Country Country { get; set; }
        [Required]
        public string StateName { get; set; } = null!;        

        [Required]
        public bool? IsActive { get; set; }
       
    }
}
