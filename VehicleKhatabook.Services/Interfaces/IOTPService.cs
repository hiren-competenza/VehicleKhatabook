namespace VehicleKhatabook.Services.Interfaces
{
    public interface IOTPService
    {
        string GenerateOTP();
        Task SaveOTPForUser(Guid userId, string otp);
        bool ValidateOTP(Guid userId, string otp);
    }
}
