using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class UserIncome : EntityBase
    {
        [Key]
        public int IncomeID { get; set; }
        [Required]
        public int IncomeCategoryID { get; set; }
        public Guid UserID { get; set; }
        public DateTime IncomeDate { get; set; } = DateTime.Now;
        public decimal IncomeAmount { get; set; }
        public string IncomeDescription { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("IncomeCategoryID")]
        public IncomeCategory IncomeCategory { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
