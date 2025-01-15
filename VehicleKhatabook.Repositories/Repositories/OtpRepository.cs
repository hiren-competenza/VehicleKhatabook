using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public OtpRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task SaveOtpAsync(OtpRequest otpRequest)
        {
            _context.OtpRequests.Add(otpRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<OtpRequest> GetOtpByUserIdAndCodeAsync(Guid userId, string otpCode, string otpRequestId)
        {
            return await _context.OtpRequests
                .FirstOrDefaultAsync(o => o.UserID == userId && o.OtpCode == otpCode && o.OtpRequestId == new Guid(otpRequestId));
        }

        public async Task<OtpRequest> GetOtpByMobileAndCodeAsync(string mobileNumber, string otpCode, string otpRequestId)
        {
            return await _context.OtpRequests
                .FirstOrDefaultAsync(o => o.MobileNumber == mobileNumber && o.OtpCode == otpCode && o.OtpRequestId == new Guid(otpRequestId));
        }

        public async Task UpdateOtpAsync(OtpRequest otpRequest)
        {
            _context.OtpRequests.Update(otpRequest);
            await _context.SaveChangesAsync();
        }
        public async Task SendOtpAsync(string mobileNumber, string otp, string SmsPurpose, string app_signature)
        {
            // Retrieve SMS settings from the database
            var smsConfig = await _context.ApplicationConfigurations.FirstOrDefaultAsync();
            if (smsConfig == null)
            {
                throw new InvalidOperationException("SMS configuration is not available.");
            }
            smsConfig.SmsText = smsConfig.SmsText.Replace("{otp}", otp);
            smsConfig.SmsText = String.IsNullOrEmpty(SmsPurpose) ? smsConfig.SmsText = smsConfig.SmsText : smsConfig.SmsText.Replace("Login", SmsPurpose);
            smsConfig.SmsText = String.IsNullOrEmpty(app_signature) ? smsConfig.SmsText = smsConfig.SmsText : smsConfig.SmsText.Replace("{app_signature}", app_signature);
            // Base URL
            var queryParams = new List<string>
             {
                 $"phone={mobileNumber}",
                 $"text={smsConfig.SmsText}"
             };

            // Add parameters only if they have values
            if (!string.IsNullOrEmpty(smsConfig.SmsApiUrl))
            {
                queryParams.Add($"user={smsConfig.SmsUser}");
                queryParams.Add($"pass={smsConfig.SmsPassword}");
                queryParams.Add($"sender={smsConfig.SmsSender}");
                queryParams.Add($"priority={smsConfig.SmsPriority}");
                queryParams.Add($"stype={smsConfig.SmsStype}");
            }

            // Construct the full URL
            string apiUrlWithParams = $"{smsConfig.SmsApiUrl}?{string.Join("&", queryParams)}";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Send the HTTP GET request to the SMS API
                    var response = await httpClient.GetAsync(apiUrlWithParams);

                    if (!response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        throw new InvalidOperationException($"Failed to send OTP via SMS. Response: {responseContent}");
                    }

                    Console.WriteLine("OTP sent successfully via SMS.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while sending OTP: {ex.Message}");
                    throw;
                }
            }
        }
        //public async Task SendOtpAsync(string mobileNumber, string otp)
        //{
        //    // Retrieve SMS settings from the database
        //    var smsConfig = await _context.ApplicationConfigurations.FirstOrDefaultAsync();
        //    if (smsConfig == null)
        //    {
        //        throw new InvalidOperationException("SMS configuration is not available.");
        //    }
        //    //string smsApiUrl = $"https://bhashsms.com/api/sendmsg.php";
        //    //string apiUrlWithParams = $"{smsApiUrl}?user=BehraEnterprises&pass=123456&sender=VBEHRA&phone={dto.MobileNumber}&text=Dear User Your OTP For Login to Vehicle Khatabook is {otp.OtpCode} Valid for 10 minutes - BEHRA ENTERPRISES&priority=ndnd&stype=normal";
        //    // Construct the SMS API URL with parameters
        //    string apiUrlWithParams = $"{smsConfig.SmsApiUrl}?user={smsConfig.SmsUser}&pass={smsConfig.SmsPassword}&sender={smsConfig.SmsSender}" +
        //                              $"&phone={mobileNumber}&text=Dear User Your OTP For Login to Vehicle Khatabook is {otp} Valid for 10 minutes - BEHRA ENTERPRISES&priority={smsConfig.SmsPriority}&stype={smsConfig.SmsStype}";

        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        try
        //        {
        //            // Send the HTTP GET request to the SMS API
        //            var response = await httpClient.GetAsync(apiUrlWithParams);

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                string responseContent = await response.Content.ReadAsStringAsync();
        //                throw new InvalidOperationException($"Failed to send OTP via SMS. Response: {responseContent}");
        //            }

        //            Console.WriteLine("OTP sent successfully via SMS.");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"An error occurred while sending OTP: {ex.Message}");
        //            throw;
        //        }
        //    }
        //}
    }
}
