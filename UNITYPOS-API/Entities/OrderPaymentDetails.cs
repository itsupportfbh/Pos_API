using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace UNITYPOS_API.Entities
{
    public class OrderPaymentDetails
    {

        [Key]
        public long Id { get; set; }

        public long PaymentId { get; set; }

        public string PaymentMode { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? ReferenceNo { get; set; }
        public string? TransactionId { get; set; }
        public string? CardNumber { get; set; }
        public string? Remarks { get; set; }
        public string PaymentStatus { get; set; } = "Success";

        public int OrgId { get; set; }
        public int BranchId { get; set; }

        [ForeignKey(nameof(PaymentId))]
        [JsonIgnore]
        public Orderpayments? Orderpayments { get; set; }
    }
}

