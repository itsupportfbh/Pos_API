using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class OrderHoldItems:CommonClass
    {

        [Key]
        public long Itemid { get; set; }

        public long Orderid { get; set; }

        [StringLength(50)]
        public string? Menuitemid { get; set; }

        [Required]
        [StringLength(255)]
        public string Itemname { get; set; } 

        
        public decimal Quantity { get; set; } = 1.00m;

      
        public decimal Unitprice { get; set; } = 0.00m;

      
        public decimal Totalprice { get; set; } = 0.00m;

      
        public decimal? DiscountAmount { get; set; } = 0.00m;

       
        public decimal? TaxAmount { get; set; } = 0.00m;

        public string? Modifierdetails { get; set; }

        [StringLength(20)]
        public string? Itemstatus { get; set; } 

       
        public string? Notes { get; set; }

        public int OrgId { get; set; }

       

      
        public OrdersHold? OrderHold { get; set; }
    }
}
