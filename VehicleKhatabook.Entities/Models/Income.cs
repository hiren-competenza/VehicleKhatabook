using Bonobo.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class Income : EntityBase
    {
        [Key]
        public int IncomeID { get; set; }
        [Required]
        public int IncomeCategoryID { get; set; }

        public string IncomeDescription { get; set; }
        public decimal IncomeAmount { get; set; }
        public DateTime IncomeDate { get; set; } = DateTime.Now;

        public Guid DriverID { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("IncomeCategoryID")]
        public IncomeCategory IncomeCategory { get; set; }

        [ForeignKey("DriverID")]
        public User Driver { get; set; }
    }
}
