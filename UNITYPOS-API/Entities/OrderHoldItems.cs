using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class OrderHoldItems
    {

        [Key]
        public long Itemid { get; set; }

        public long Orderid { get; set; }

       
        public int? Menuitemid { get; set; }
        public int? ComboMenuItemId { get; set; }

        [Required]
        [StringLength(255)]
        public string Itemname { get; set; } 

        
        public decimal Quantity { get; set; } = 1.00m;

      
        public decimal Unitprice { get; set; } = 0.00m;

      
        public decimal Totalprice { get; set; } = 0.00m;

      
        public decimal? DiscountAmount { get; set; } = 0.00m;

       
        public decimal? TaxAmount { get; set; } = 0.00m;

        public string? Modifierdetails { get; set; }


        public string? Itemstatus { get; set; } 

       
        public string? Notes { get; set; }

        public int OrgId { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey("Orderid")]
        public OrdersHold? OrderHold { get; set; }
    }
}
