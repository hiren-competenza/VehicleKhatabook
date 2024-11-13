namespace VehicleKhatabook.Models.DTOs
{
    public class AdminUserDTO
    {
        public int AdminID { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string MobileNumber { get; set; }
        public bool? IsActive { get; set; }
    }

}
