﻿using Bonobo.Entities;
using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Entities.Models
{
    public class IncomeCategory : EntityBase
    {
        [Key]
        public int IncomeCategoryID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}