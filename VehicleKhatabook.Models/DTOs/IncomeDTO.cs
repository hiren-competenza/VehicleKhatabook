namespace VehicleKhatabook.Models.DTOs
{
    public class IncomeDTO
    {
        public Guid VehicleID { get; set; }
        public int IncomeCategoryID { get; set; }
        public string IncomeSource { get; set; }
        public decimal IncomeAmount { get; set; }
        public DateTime IncomeDate { get; set; }
        public Guid DriverID { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
