using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Orders 
    {
        [Key]
        public long Orderid { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public int? TableId { get; set; }

        public string OrderType { get; set; } = string.Empty;

        public string OrderStatus { get; set; } 

        public int? ItemCount { get; set; } = 1;

        public decimal? SubtotalAmount { get; set; } = 0.00m;

        public decimal? TaxAmount { get; set; } = 0.00m;

        public decimal? DiscountAmount { get; set; } = 0.00m;

        public decimal? TotalAmount { get; set; } = 0.00m;

        public int? ShiftId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int OrgId { get; set; }
        public int BranchId { get; set; }

      
        public List<Orderitems>? Items { get; set; }
    }
}
