using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleKhatabook.Models.DTOs
{
    public class DriverOwnerUserDTO
    {
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string MobileNumber { get; set; }
        public string UserType { get; set; }
        public bool IsActive { get; set; }
    }
}
