using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class OTPService : IOTPService
    {
        private readonly Dictionary<Guid, string> _userOtps = new Dictionary<Guid, string>();

        public string GenerateOTP()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task SaveOTPForUser(Guid userId, string otp)
        {
            _userOtps[userId] = otp;
            await Task.CompletedTask;
        }

        public bool ValidateOTP(Guid userId, string otp)
        {
            return _userOtps.ContainsKey(userId) && _userOtps[userId] == otp;
        }
    }
}
