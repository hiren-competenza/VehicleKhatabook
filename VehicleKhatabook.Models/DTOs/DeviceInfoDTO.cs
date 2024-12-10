using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleKhatabook.Models.DTOs
{
    public class DeviceInfoDTO
    {
        public Guid DeviceInfoID { get; set; }
        public string? DeviceModel { get; set; }
        public string? DeviceNumber { get; set; }
        public string? Location { get; set; }
        public string? OS { get; set; }
        public string? AppVersion { get; set; }
        public DateTime RegisteredOn { get; set; }
    }

}
