namespace VehicleKhatabook.Models.DTOs
{
    public class ExpenseCategoryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
