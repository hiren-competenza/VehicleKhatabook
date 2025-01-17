using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class SendTransactionMessageRepository : ISendTransactionMessageRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public SendTransactionMessageRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public void SendTransactionMessage(string transactionType, string userId, string driverOwnerUserId, decimal? amount)
        {
            // Get SMS configuration
            var smsConfig = _context.ApplicationConfigurations.FirstOrDefault();
            if (smsConfig == null)
            {
                throw new InvalidOperationException("SMS configuration is not available.");
            }

            // Get user
            var user = _context.Users.FirstOrDefault(u => u.UserID.ToString() == userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Calculate balance
            var creditTotal = _context.OwnerKhataCredits
                .Where(c => c.DriverOwnerId.ToString() == driverOwnerUserId)
                .Sum(c => c.Amount);

            var debitTotal = _context.OwnerKhataDebits
                .Where(d => d.DriverOwnerId.ToString() == driverOwnerUserId)
                .Sum(d => d.Amount);

            var balance = creditTotal - debitTotal;

            // Get mobile number
            var mobileNumber = _context.DriverOwnerUsers
                .Where(d => d.DriverOwnerUserId.ToString() == driverOwnerUserId)
                .Select(d => d.MobileNumber)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(mobileNumber))
            {
                throw new InvalidOperationException("Mobile number not found for the specified DriverOwnerId.");
            }

            // Prepare SMS text
            string smsText;
            if (transactionType.Equals("Credit", StringComparison.OrdinalIgnoreCase))
            {
                smsText = smsConfig.CreditTransactionSmsText.Replace("{amount}", amount.ToString())
                    .Replace("{user}", $"{user.FirstName} {user.LastName}")
                    .Replace("{balance}", balance.ToString());
            }
            else if (transactionType.Equals("Debit", StringComparison.OrdinalIgnoreCase))
            {
                smsText = smsConfig.DebitTransactionSmsText
                    .Replace("{amount}", amount.ToString())
                    .Replace("{user}", $"{user.FirstName} {user.LastName}")
                    .Replace("{balance}", balance.ToString());
            }
            else
            {
                throw new InvalidOperationException("Invalid transaction type. Allowed values are 'Credit' or 'Debit'.");
            }

            // Prepare SMS API query parameters
            var queryParams = new List<string>
    {
        $"phone={mobileNumber}",
        $"text={smsText}"
    };

            if (!string.IsNullOrEmpty(smsConfig.SmsApiUrl))
            {
                queryParams.Add($"user={smsConfig.SmsUser}");
                queryParams.Add($"pass={smsConfig.SmsPassword}");
                queryParams.Add($"sender={smsConfig.SmsSender}");
                queryParams.Add($"priority={smsConfig.SmsPriority}");
                queryParams.Add($"stype={smsConfig.SmsStype}");
            }

            // Construct API URL
            string apiUrlWithParams = $"{smsConfig.SmsApiUrl}?{string.Join("&", queryParams)}";

            // Send SMS via API
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    var response = httpClient.GetAsync(apiUrlWithParams).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result; // Blocking call
                        throw new InvalidOperationException($"Failed to send SMS. Response: {responseContent}");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"An error occurred while sending SMS: {ex.Message}", ex);
                }
            }
        }

    }
}
