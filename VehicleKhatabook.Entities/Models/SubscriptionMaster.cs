using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleKhatabook.Entities.Models
{
    [Table("SubscriptionMaster")]
    public class SubscriptionMaster : EntityBase
    {
        [Key]
        public int SubscriptionId { get; set; }

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

    }
}
