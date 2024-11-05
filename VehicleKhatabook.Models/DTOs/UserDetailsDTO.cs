namespace VehicleKhatabook.Models.DTOs
{
    public class UserDetailsDTO
    {
        public Guid UserId { get; set; }
        //public string Email { get; set; }
        //public string UserName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserReferCode { get; set; }
        //public int UserTypeId {  get; set; }
        public string MobileNumber { get; set; }
        public bool IsPremiumUser { get; set; }
        public string State { get; set;}
        public string District { get; set; }
        public bool IsActive { get; set; }
        public int? LanguageTypeId { get; set; }
        public string? Mpin {  get; set; }

        //public Guid SessionId { get; set; }
    }
}
