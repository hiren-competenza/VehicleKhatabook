using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;
using static System.Net.WebRequestMethods;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class OwnerIncomeRepository : IOwnerIncomeRepository
    {
        private readonly VehicleKhatabookDbContext _context;
        private readonly ISendTransactionMessageRepository _sendTransactionMessageRepository;

        public OwnerIncomeRepository(
            VehicleKhatabookDbContext context,
            ISendTransactionMessageRepository sendTransactionMessageRepository)
        {
            _context = context;
            _sendTransactionMessageRepository = sendTransactionMessageRepository;
        }
        public async Task<OwnerKhataCredit> AddOwnerIncomeAsync(OwnerIncomeExpenseDTO incomeDTO)
        {
            var income = new OwnerKhataCredit
            {
                //Name = incomeDTO.Name,
                //UserId = incomeDTO.UserId,
                //Mobile = incomeDTO.Mobile,
                Date = incomeDTO.Date,
                Amount = incomeDTO.Amount,
                Note = incomeDTO.Note,
                DriverOwnerId = incomeDTO.DriverOwnerUserId,
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow                
            };
            _context.OwnerKhataCredits.Add(income);
            await _context.SaveChangesAsync();
            await _context.Entry(income).Reference(i => i.DriverOwnerUser).LoadAsync();

            // Load Vehicle's related User details
            if (income.DriverOwnerUser != null)
            {
                await _context.Entry(income.DriverOwnerUser).Reference(v => v.user).LoadAsync();
            }
            try
            {
                _sendTransactionMessageRepository.SendTransactionMessage(
                    incomeDTO.TransactionType,
                    incomeDTO.Id.ToString(),
                    income.DriverOwnerId.ToString(),
                    income.Amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending transaction message: {ex.Message}");
            }
            return income;
        }

        public async Task<List<OwnerIncomeExpenseDTO>> GetOwnerIncomeAsync(Guid driverOwnerUserId, DateTime fromDate, DateTime toDate)
        {
            var result = await _context.OwnerKhataCredits
                .Where(e => e.DriverOwnerId == driverOwnerUserId && e.Date >= fromDate && e.Date <= toDate)
                .Include(e => e.DriverOwnerUser)            // Include related Vehicle details
                    .ThenInclude(v => v.user)
                .OrderByDescending(i => i.Date)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();
            return result.Select(MapToDTO).ToList();
        }

        public async Task<List<OwnerIncomeExpenseDTO>> GetOwnerIncomeAsync(Guid driverOwnerUserId)
        {
            var result = await _context.OwnerKhataCredits
                .Where(e => e.DriverOwnerId == driverOwnerUserId)
                .Include(e => e.DriverOwnerUser)            // Include related Vehicle details
                    .ThenInclude(v => v.user)
                .OrderByDescending(i => i.Date)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();
            return result.Select(MapToDTO).ToList();
        }

        //public async Task<List<OwnerIncomeExpenseDTO)>> GetOwnerIncomebyUserAsync(Guid userId)
        //{
        //    var result = await _context.OwnerKhataCredits
        //        .Where(e => e.DriverOwnerUser.UserID == userId)
        //        .Include(e => e.DriverOwnerUser)            // Include related Vehicle details
        //            .ThenInclude(v => v.user)
        //        .OrderByDescending(i => i.Date)
        //        .ThenByDescending(i => i.CreatedOn)
        //        .ToListAsync();
        //    return MapToDTO(result);
        //}
        public async Task<List<OwnerIncomeExpenseDTO>> GetOwnerIncomebyUserAsync(Guid userId)
        {
            var result = await _context.OwnerKhataCredits
                .Where(e => e.DriverOwnerUser.UserID == userId)
                .Include(e => e.DriverOwnerUser) // Include related Vehicle details
                    .ThenInclude(v => v.user)
                .OrderByDescending(i => i.Date)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();

            return result.Select(MapToDTO).ToList();
        }

        public async Task<ApiResponse<OwnerKhataCredit>> GetOwnerIncomeDetailsAsync(Guid id)
        {
            var income = await _context.OwnerKhataCredits.FindAsync(id);
            return income != null ? ApiResponse<OwnerKhataCredit>.SuccessResponse(income, "Income details retrieved successfully.") : ApiResponse<OwnerKhataCredit>.FailureResponse("Income not found");

        }

        public async Task<bool> AccountSettlementIncomeAsync(Guid driverOwnerUserId, Guid userId)
        {
            var dataToDelete = _context.OwnerKhataCredits.Where(debit => debit.DriverOwnerId == driverOwnerUserId && debit.DriverOwnerUser.UserID == userId);
            // Remove all records that match the condition
            if (!dataToDelete.Any())
            {
                return true; // No data found to delete
            }
            _context.OwnerKhataCredits.RemoveRange(dataToDelete);
            // Save changes to the database
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OwnerKhataCredit> UpdateOwnerIncomeAsync(Guid userId, OwnerIncomeExpenseDTO incomeDTO)
        {
            var income = await _context.OwnerKhataCredits
                .FirstOrDefaultAsync(i => i.Id == userId);

            if (income == null)
            {
                throw new KeyNotFoundException("Income record not found.");
            }
            income.Amount = incomeDTO.Amount;
            income.Note = incomeDTO.Note;

            _context.OwnerKhataCredits.Update(income);
            await _context.SaveChangesAsync();
            if (income.DriverOwnerUser != null)
            {
                await _context.Entry(income.DriverOwnerUser).Reference(v => v.user).LoadAsync();
            }
            return income;
        }

        public async Task<bool> DeleteOwnerIncomeAsync(Guid userId)
        {
            var income = await _context.OwnerKhataCredits
                .FirstOrDefaultAsync(i => i.Id == userId);

            if (income == null)
            {
                return false;
            }
            _context.OwnerKhataCredits.Remove(income);
            await _context.SaveChangesAsync();

            return true;
        }
        private OwnerIncomeExpenseDTO MapToDTO(OwnerKhataCredit ownerKhataCredit)
        {
            return new OwnerIncomeExpenseDTO
            {
                Id = ownerKhataCredit.Id,
                Amount = ownerKhataCredit.Amount,
                Date = ownerKhataCredit.Date,
                Note = ownerKhataCredit.Note,
                DriverOwnerUserId = ownerKhataCredit.DriverOwnerId,
                TransactionType = "credit",
                TransactionDate = ownerKhataCredit.Date
            };
        }
    //    public void SendTransactionMessage(string TransactionType, string userId, string DriverOwnerUserId, decimal? Amount)
    //    {
    //        var smsConfig = _context.ApplicationConfigurations.FirstOrDefault();
    //        if (smsConfig == null)
    //        {
    //            throw new InvalidOperationException("SMS configuration is not available.");
    //        }

    //        var user = _context.Users
    //            .Where(u => u.UserID.ToString() == userId)
    //            .FirstOrDefault();

    //        if (user == null)
    //        {
    //            throw new InvalidOperationException("User not found.");
    //        }

    //        var creditTotal = _context.OwnerKhataCredits
    //            .Where(c => c.DriverOwnerId.ToString() == DriverOwnerUserId)
    //            .Sum(c => c.Amount);

    //        var debitTotal = _context.OwnerKhataDebits
    //            .Where(d => d.DriverOwnerId.ToString() == DriverOwnerUserId)
    //            .Sum(d => d.Amount);

    //        var balance = creditTotal - debitTotal;

    //        var mobileNumber = _context.DriverOwnerUsers
    //            .Where(d => d.DriverOwnerUserId.ToString() == DriverOwnerUserId)
    //            .Select(d => d.MobileNumber)
    //            .FirstOrDefault();

    //        if (mobileNumber == null)
    //        {
    //            throw new InvalidOperationException("Mobile number not found for the specified DriverOwnerId.");
    //        }

    //        if (TransactionType == "Credit")
    //        {
    //            smsConfig.CreditTransactionSmsText = smsConfig.CreditTransactionSmsText.Replace("{amount}", Amount.ToString());

    //            smsConfig.CreditTransactionSmsText = smsConfig.CreditTransactionSmsText.Replace("{user}", $"{user.FirstName} {user.LastName}");

    //            smsConfig.CreditTransactionSmsText = smsConfig.CreditTransactionSmsText.Replace("{balance}", balance.ToString());
    //        }
    //        else if (TransactionType == "Debit")
    //        {
    //            smsConfig.DebitTransactionSmsText = smsConfig.CreditTransactionSmsText
    //                .Replace("{amount}", Amount.ToString())
    //                .Replace("{userName}", $"{user.FirstName} {user.LastName}")
    //                .Replace("{balance}", balance.ToString());
    //        }
    //        else
    //        {
    //            throw new InvalidOperationException("Invalid transaction type.");
    //        }

    //        var smsText = TransactionType == "credit" ? smsConfig.CreditTransactionSmsText : smsConfig.DebitTransactionSmsText;

    //        // Base URL
    //        var queryParams = new List<string>
    //{
    //    $"phone={mobileNumber}",
    //    $"text={smsText}"
    //};

    //        // Add parameters only if they have values
    //        if (!string.IsNullOrEmpty(smsConfig.SmsApiUrl))
    //        {
    //            queryParams.Add($"user={smsConfig.SmsUser}");
    //            queryParams.Add($"pass={smsConfig.SmsPassword}");
    //            queryParams.Add($"sender={smsConfig.SmsSender}");
    //            queryParams.Add($"priority={smsConfig.SmsPriority}");
    //            queryParams.Add($"stype={smsConfig.SmsStype}");
    //        }

    //        // Construct the full URL
    //        string apiUrlWithParams = $"{smsConfig.SmsApiUrl}?{string.Join("&", queryParams)}";

    //        using (HttpClient httpClient = new HttpClient())
    //        {
    //            try
    //            {
    //                // Send the HTTP GET request to the SMS API
    //                var response = httpClient.GetAsync(apiUrlWithParams).Result;

    //                if (!response.IsSuccessStatusCode)
    //                {
    //                    string responseContent = response.Content.ReadAsStringAsync().Result;
    //                    throw new InvalidOperationException($"Failed to send transaction message via SMS. Response: {responseContent}");
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine($"An error occurred while sending OTP: {ex.Message}");
    //                throw;
    //            }
    //        }
    //    }


    }
}
