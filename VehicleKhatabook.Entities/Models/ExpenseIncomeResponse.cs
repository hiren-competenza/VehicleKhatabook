namespace VehicleKhatabook.Entities.Models
{
    public class ExpenseIncomeResponse
    {
        public List<CategoryDTO> ExpenseCategory { get; set; }
        public List<CategoryDTO> IncomeCategory { get; set; }
    }
}
