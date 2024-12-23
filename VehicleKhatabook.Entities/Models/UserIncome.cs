﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Entities.Models
{
    public class UserIncome : EntityBase, IHasTransactionDate
    {
        [Key]
        public int IncomeID { get; set; }
        [Required]
        public int IncomeCategoryID { get; set; }
        public Guid IncomeVehicleId { get; set; }
        //public Guid UserID { get; set; }
        public DateTime IncomeDate { get; set; } = DateTime.Now;
        public decimal IncomeAmount { get; set; }
        public string? IncomeDescription { get; set; }
        public bool? IsActive { get; set; }
        [ForeignKey("IncomeCategoryID")]
        public IncomeCategory IncomeCategory { get; set; }
        public Vehicle Vehicle { get; set; }
        DateTime IHasTransactionDate.TransactionDate => IncomeDate;

    }
}
