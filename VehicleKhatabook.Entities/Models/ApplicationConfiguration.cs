using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VehicleKhatabook.Entities.Models
{
    public class ApplicationConfiguration : EntityBase
    {
        [Key]
        public Guid ApplicationConfigurationId { get; set; } = Guid.NewGuid();

        // SMS Configuration
        [Display(Name = "SMS API URL")]
        public string? SmsApiUrl { get; set; }

        [Display(Name = "SMS User")]
        public string? SmsUser { get; set; }

        [Display(Name = "SMS Password")]
        public string? SmsPassword { get; set; }

        [Display(Name = "SMS Sender")]
        public string? SmsSender { get; set; }

        [Display(Name = "SMS Priority")]
        public string? SmsPriority { get; set; }

        [Display(Name = "SMS Stype")]
        public string? SmsStype { get; set; }

        [Display(Name = "SMS Text")]
        public string? SmsText { get; set; }

        // Contact Information
        [Display(Name = "Support Email")]
        public string? SupportEmail { get; set; }

        [Display(Name = "Support WhatsApp Number")]
        public string? SupportWhatsAppNumber { get; set; }

        // Payment Gateway Configuration (Stripe)
        [Display(Name = "Payment Gateway API Key")]
        public string? PaymentGatewayApiKey { get; set; }

        [Display(Name = "Payment Gateway Public Key")]
        public string? PaymentGatewayPublicKey { get; set; }

        [Display(Name = "Payment Gateway Webhook Secret")]
        public string? PaymentGatewayWebhookSecret { get; set; }

        [Display(Name = "Payment Gateway Currency")]
        public string? PaymentGatewayCurrency { get; set; }

        [Display(Name = "Payment Gateway Name")]
        public string? PaymentGatewayName { get; set; }

        [Display(Name = "Payment Gateway Status")]
        public string? PaymentGatewayStatus { get; set; }

        // Subscription Configuration
        [Display(Name = "Subscription Name")]
        public string? SubscriptionName { get; set; }

        [Display(Name = "Subscription Amount")]
        public string? SubscriptionAmount { get; set; }

        [Display(Name = "Subscription Duration (Days)")]
        public string? SubscriptionDurationDays { get; set; }

        [Display(Name = "Is Subscription Renewable")]
        public string? SubscriptionIsRenewable { get; set; }

        [Display(Name = "Subscription Renewal Reminder (Days Before)")]
        public string? SubscriptionRenewalReminderDaysBefore { get; set; }

        [Display(Name = "Subscription Trial Period (Days)")]
        public string? SubscriptionTrialPeriodDays { get; set; }

        // Social Media Configuration
        [Display(Name = "Facebook Page URL")]
        public string? FacebookPageUrl { get; set; }

        [Display(Name = "Twitter Handle")]
        public string? TwitterHandle { get; set; }

        [Display(Name = "Instagram Handle")]
        public string? InstagramHandle { get; set; }

        [Display(Name = "LinkedIn URL")]
        public string? LinkedInUrl { get; set; }

        [Display(Name = "YouTube Channel URL")]
        public string? YouTubeChannelUrl { get; set; }

        [Display(Name = "Pinterest URL")]
        public string? PinterestUrl { get; set; }

        [Display(Name = "Ad ShowOn Page")]
        public string? AdShowOnPage { get; set; }

        // IsActive
        [Display(Name = "Is Active")]
        public string? IsActive { get; set; }

        public List<(string FieldName, string Label, object? Value)> GetConfigurationFields(ApplicationConfiguration config)
        {
            return typeof(ApplicationConfiguration).GetProperties()
                .Select(p => (
                    FieldName: p.Name,
                    Label: p.GetCustomAttribute<DisplayAttribute>()?.Name ?? p.Name,
                    Value: p.GetValue(config)
                ))
                .ToList();
        }


    }

}
