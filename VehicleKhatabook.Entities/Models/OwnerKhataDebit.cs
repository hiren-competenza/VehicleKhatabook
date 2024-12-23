﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class OwnerKhataDebit : EntityBase, IHasTransactionDate
    {
        [Key]
        public Guid Id { get; set; }
        //public Guid UserId { get; set; }
        public Guid DriverOwnerId { get; set; }
        //public string? Name { get; set; }
        //public string? Mobile { get; set; }
        public DateTime Date { get; set; }
        public decimal? Amount { get; set; }
        public string? Note { get; set; }

        //[ForeignKey("UserId")]
        // public User User { get; set; }
        public DriverOwnerUser DriverOwnerUser { get; set; }

        DateTime IHasTransactionDate.TransactionDate => Date;

    }
}
