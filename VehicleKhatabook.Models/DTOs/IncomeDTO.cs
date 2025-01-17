﻿namespace VehicleKhatabook.Models.DTOs
{
    public class IncomeDTO
    {
        public int IncomeCategoryID { get; set; }
        public decimal IncomeAmount { get; set; }
        public DateTime IncomeDate { get; set; }
        public string IncomeDescription { get; set; }
        //public Guid UserId { get; set; }
        public Guid IncomeVehicleId { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
