using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    public class FirebaseSettings
    {
        public string ServerKey { get; set; }
        public string SenderId { get; set; }
    }

}
