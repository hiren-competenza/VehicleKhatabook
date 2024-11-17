namespace VehicleKhatabook.Models.DTOs
{
    public class ExpenseDTO
    {
        public int ExpenseCategoryID { get; set; }
        public decimal ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ExpenseDescription { get; set; }
        //public Guid UserId { get; set; }
        public Guid ExpenseVehicleId { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
