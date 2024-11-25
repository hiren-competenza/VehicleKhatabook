using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleKhatabook.Entities.Models
{
    public class ApplicationConfiguration : EntityBase
    {
        [Key]
        public Guid ApplicationConfigurationId { get; set; } = Guid.NewGuid();

        // SMS Configuration
        public string? SMSApiKey { get; set; }
        public string? SMSApiUrl { get; set; }
        public string? SMSSenderId { get; set; }

        // Contact Information
        public string? SupportEmail { get; set; }
        public string? SupportWhatsAppNumber { get; set; }

        // Payment Gateway Configuration (Stripe)
        public string? PaymentGatewayApiKey { get; set; }
        public string? PaymentGatewayPublicKey { get; set; }
        public string? PaymentGatewayWebhookSecret { get; set; }
        public string? PaymentGatewayCurrency { get; set; }
        public string? PaymentGatewayName { get; set; }
        public bool? PaymentGatewayStatus { get; set; }

        // Subscription Configuration
        public string? SubscriptionName { get; set; }
        public decimal? SubscriptionAmount { get; set; }
        public int? SubscriptionDurationDays { get; set; }
        public bool? IsRenewable { get; set; }
        public int? RenewalReminderDaysBefore { get; set; }
        public int? TrialPeriodDays { get; set; }

        // Social Media Configuration
        public string? FacebookPageUrl { get; set; }
        public string? TwitterHandle { get; set; }
        public string? InstagramHandle { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? YouTubeChannelUrl { get; set; }
        public string? PinterestUrl { get; set; }

        // IsActive
        public bool? IsActive { get; set; }

    }

}
