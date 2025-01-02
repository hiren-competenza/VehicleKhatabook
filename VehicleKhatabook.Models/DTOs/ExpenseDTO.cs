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
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
