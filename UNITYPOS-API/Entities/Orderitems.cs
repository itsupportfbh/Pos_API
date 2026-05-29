using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Orderitems
    {
              
        [Key]
        public long Itemid { get; set; }

        public long Orderid { get; set; }
        
        public int? Menuitemid { get; set; }

        public int? ComboMenuItemId { get; set; }
        public string Itemname { get; set; } = string.Empty;

       
        public decimal Quantity { get; set; } 

       
        public decimal Unitprice { get; set; } 

      
        public decimal Totalprice { get; set; } 

        public decimal? DiscountAmount { get; set; } 

       
        public decimal? TaxAmount { get; set; } 

        public string? Modifierdetails { get; set; }

        
        public int? Itemstatus { get; set; } 

       
        public string? Notes { get; set; }

        public int OrgId { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        //public int BranchId { get; set; }

        public Orders? Order { get; set; }
    }
}

