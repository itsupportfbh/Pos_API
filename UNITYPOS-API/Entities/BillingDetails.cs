using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class BillingDetails:CommonClass
    {
        [Key]
        public int Id { get; set; }

        public int BillingId { get; set; }

        public string PaymentMode { get; set; } = string.Empty;
        public decimal GrossAmount { get; set; }
        public string? ReferenceNo { get; set; }
              
        public string? TransactionId { get; set; }
              
        public string? CardNumber { get; set; }
        public decimal TaxableAmount { get; set; }

        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }

        public decimal SGSTPercentage { get; set; }
        public decimal SGSTAmount { get; set; }

        public decimal CGSTPercentage { get; set; }
        public decimal CGSTAmount { get; set; }

        public decimal IGSTPercentage { get; set; }
        public decimal IGSTAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Remarks { get; set; }

        
        public int PaymentStatus { get; set; } 
        public bool IsDeleted { get; set; }
        public int OrgId { get; set; }
        public int BranchId { get; set; }

        
    }
}
