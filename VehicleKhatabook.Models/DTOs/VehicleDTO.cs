using FluentValidation;
using System.Text.Json.Serialization;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Models.DTOs
{
    public class VehicleDTO
    {
        public Guid UserId { get; set; }
        public int? VehicleTypeId { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? NickName { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? InsuranceExpiry { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? PollutionExpiry { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? FitnessExpiry { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? RoadTaxExpiry { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? RCPermitExpiry { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? NationalPermitExpiry { get; set; }
        public string? ChassisNumber { get; set; }
        public string? EngineNumber { get; set; }
        [JsonConverter(typeof(NullableBoolConverter))]
        public bool? IsActive { get; set; }
    }
    public class AddVehicleValidator : AbstractValidator<VehicleDTO>
    {
        public AddVehicleValidator()
        {
            //RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required. Please enter a valid user.");
            RuleFor(x => x.VehicleTypeId).NotEmpty().WithMessage("Vehicle type is required. Please select a vehicle type.");
        }
    }
}
