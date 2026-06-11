using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Billing: CommonClass
    {

        [Key]
        public int Id { get; set; }
        public string BillNo { get; set; } = string.Empty;

        public int OrderId { get; set; }

        public int? CustomerId { get; set; }

        public DateTime BillDate { get; set; } = DateTime.Now;

        public int? TokenNo { get; set; }

        // Bill Values
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TipAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal TotalAmount { get; set; }

        // Payment Summary
        public decimal ReceivedAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal ChangeAmount { get; set; }

        // DineIn / TakeAway / Delivery
        
        public string BillMode { get; set; } = "DineIn";

        // Paid / Partial / Pending / Refunded
       
        public int  PaymentStatus { get; set; } 

        // Cash / Card / UPI / Multi
       
        public string PaymentType { get; set; } = "Cash";

        public string? Remarks { get; set; }

        public int OrgId { get; set; }
        public int BranchId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public List<BillingDetails> BillingDetails { get; set; } = new();

        [NotMapped]
        public int EntityNo { get; set; }
        [NotMapped]
        public List<int>? OrderIds { get; set; }
        [NotMapped]
        public List<Orderitems>? OrderItems { get; set; }
    }
}
