using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Models.DTOs
{
    public class OwnerIncomeExpenseDTO
    {
        public Guid? Id { get; set; }
        //public Guid UserId { get; set; }
        public Guid DriverOwnerUserId { get; set; }
        //[JsonConverter(typeof(NullableStringConverter))]
        //public string? Name { get; set; }
        //[JsonConverter(typeof(NullableStringConverter))]
        //public string? Mobile { get; set; }
        public DateTime Date { get; set; }
        public decimal? Amount { get; set; }
        [JsonConverter(typeof(NullableStringConverter))]
        public string? Note { get; set; }

    }
}
