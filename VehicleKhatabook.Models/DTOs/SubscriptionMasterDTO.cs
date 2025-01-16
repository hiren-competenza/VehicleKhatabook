using System.ComponentModel.DataAnnotations;

namespace VehicleKhatabook.Models.DTOs
{
    public class SubscriptionMasterDTO
    {
        public string? SubscriptionName { get; set; }
        public string? SubscriptionAmount { get; set; }
        public string? SubscriptionDurationDays { get; set; }
        public string? SubscriptionIsRenewable { get; set; }
        public string? SubscriptionRenewalReminderDaysBefore { get; set; }
        public string? SubscriptionTrialPeriodDays { get; set; }
    }
}
