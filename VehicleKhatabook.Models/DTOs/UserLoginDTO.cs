﻿namespace VehicleKhatabook.Models.DTOs
{
    public class UserLoginDTO
    {
        public string MobileNumber { get; set; }
        public string mPIN { get; set; }
        public string deviceType { get; set; }
        public string firebaseToken { get; set; }

        public int UserTypeId { get; set; }
    }
}
