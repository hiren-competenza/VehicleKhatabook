using System.Net.Mail;
using System.Net;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendOtpEmailAsync(string email, string otp)
        {
            // Use Gmail's SMTP server
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("choudhary.mukesh271@gmail.com", "password"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("choudhary.mukesh271@gmail.com"),
                Subject = "Reset your mPIN - OTP",
                Body = $"Your OTP for resetting mPIN is: {otp}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("OTP email sent successfully.");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Error sending OTP email: {ex.Message}");
            }
        }
    }
}
