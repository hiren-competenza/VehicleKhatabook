namespace VehicleKhatabook.Models.DTOs
{
    public class OwnerKhataDTO
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public bool IsCredit { get; set; }
    }
}
