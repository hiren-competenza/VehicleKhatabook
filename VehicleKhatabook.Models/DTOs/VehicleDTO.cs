using FluentValidation;

namespace VehicleKhatabook.Models.DTOs
{
    public class VehicleDTO
    {
        public Guid UserId { get; set; }
        public string? VehicleType { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? NickName { get; set; }
        public DateTime InsuranceExpiry { get; set; }
        public DateTime PollutionExpiry { get; set; }
        public DateTime FitnessExpiry { get; set; }
        public DateTime RoadTaxExpiry { get; set; }
        public DateTime RCPermitExpiry { get; set; }
        public DateTime NationalPermitExpiry { get; set; }
        public string? ChassisNumber { get; set; }
        public string? EngineNumber { get; set; }
        public bool IsActive { get; set; }
    }
    public class AddVehicleValidator : AbstractValidator<VehicleDTO>
    {
        public AddVehicleValidator() 
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Invalid UserId");
            RuleFor(x => x.VehicleType).NotEmpty().WithMessage("Invalid VehicleType");
        }
    }
}
