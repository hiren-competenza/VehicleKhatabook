using FluentValidation;
using System.Text.Json.Serialization;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Models.DTOs
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string MobileNumber { get; set; }
        public string ReferCode { get; set; }
        public int? ReferCodeCount { get; set; }
        public string UserReferCode { get; set; }
        public string Role { get; set; }
        public bool? IsPremiumUser { get; set; }

        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? PremiumStartDate { get; set; }

        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? PremiumExpiryDate { get; set; }

        public string State { get; set; }
        public string District { get; set; }
        public int? LanguageTypeId { get; set; }
        public bool? IsActive { get; set; }
        public string? mPIN { get; set; }
        public Guid UserId { get; set; }
        public int UserTypeId { get; set; }
        public string? Email { get; set; }

        // Change this to a single DeviceInfo instead of a list
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DeviceInfoDTO? DeviceInfo { get; set; }  // Single device info instead of a list
    }

    public class AddUserValidator : AbstractValidator<UserDTO>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required");
            RuleFor(x => x.MobileNumber).NotEmpty().WithMessage("Mobile Number is required");
            RuleFor(x => x.Role).NotEmpty().WithMessage("Role Must Not Empty");
        }
    }
}
