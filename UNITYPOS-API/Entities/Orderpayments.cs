using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Orderpayments:CommonClass
    {
        [Key]
        public long Id { get; set; }
        public string Code { get; set; }

        public long OrderId { get; set; }

        // Customer
        public int? CustomerId { get; set; }

        // Bill Values
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TipAmount { get; set; }

        public decimal GrandTotal { get; set; }

        // Payment Summary
        public decimal ReceivedAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal ChangeAmount { get; set; }

        // Billing /Take Away/DinIn
        public string BillMode { get; set; }

        // Paid / Partial / Pending / Refunded
        public string PaymentStatus { get; set; } 

        // Cash / Card / Multi Payment
        public string PaymentType { get; set; }

        public string? Remarks { get; set; }

        public int OrgId { get; set; }
        public int BranchId { get; set; }

        public bool IsDeleted { get; set; }
        public List<OrderPaymentDetails>? PaymentDetails { get; set; }

        [NotMapped]
        public int EntityNo { get; set; }
    }
}
