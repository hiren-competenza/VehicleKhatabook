namespace VehicleKhatabook.Models.DTOs
{
    public class IncomeDTO
    {
        public int IncomeCategoryID { get; set; }
        public decimal IncomeAmount { get; set; }
        public DateTime IncomeDate { get; set; }
        public string IncomeDescription { get; set; }
        public Guid DriverID { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
