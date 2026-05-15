using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class OrdersHold
    {

        [Key]
        public long OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string Ordernumber { get; set; } = string.Empty;

      
        public int? Tableid { get; set; }

        public string Ordertype { get; set; } 

        
        public string Orderstatus { get; set; } 

        public int? Itemcount { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? SubtotalAmount { get; set; } = 0.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TaxAmount { get; set; } = 0.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? DiscountAmount { get; set; } = 0.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalAmount { get; set; } = 0.00m;

    
        public int? Shiftid { get; set; }

        public int OrgId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int BranchId { get; set; }

        [NotMapped]
        public List<OrderHoldItems>? Items { get; set; }
    }
}
