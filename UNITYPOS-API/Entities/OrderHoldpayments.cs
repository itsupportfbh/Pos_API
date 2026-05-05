using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class OrderHoldpayments:CommonClass
    {
        [Key]
        public long Paymentid { get; set; }

        public long Orderid { get; set; }

        [Required]
        [StringLength(30)]
        public string Paymenttype { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [StringLength(100)]
        public string? Referenceno { get; set; }

        [StringLength(20)]
        public string? Paymentstatus { get; set; } = "SUCCESS";

        public int OrgId { get; set; }

        public bool? IsDeleted { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [ForeignKey(nameof(Orderid))]
        public OrdersHold? OrderHold { get; set; }
    }
}
