namespace VehicleKhatabook.Models.DTOs
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string ReferCode { get; set; }
        public string Role { get; set; }
        public bool IsPremiumUser { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Language { get; set; }
        public bool IsActive { get; set; }
        public string? mPIN { get; set; }
        public Guid UserId { get; set; }
    }
}
