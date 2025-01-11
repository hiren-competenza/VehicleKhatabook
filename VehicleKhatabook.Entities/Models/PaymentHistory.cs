using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VehicleKhatabook.Entities.Models
{
    public class PaymentHistory : EntityBase
    {
        [Key]
        public string PaymentId { get; set; } = Guid.NewGuid().ToString();

        public String? UserId { get; set; }
        public string? TransactionId { get; set; }

        public int? PackageId { get; set; } 

        [ForeignKey(nameof(PackageId))]
        public SubscriptionMaster? Subscription { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public int? Validity { get; set; }
        public string? CardNumberLast4 { get; set; }
        public string? CardExpiry { get; set; }
        public string? BillingAddress { get; set; }
        public string? PayerEmail { get; set; }
        public string? PayerName { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? GatewayResponse { get; set; }
        public string? GatewayId { get; set; }
        public string? SubscriptionId { get; set; }
        public string? SubscriptionType { get; set; }
        public string? FailureReason { get; set; }
        public string? Notes { get; set; }
    }

}
