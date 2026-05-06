using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNITYPOS_API.Entities
{
    public class Orderitems
    {
              
        [Key]
        public long Itemid { get; set; }

        public long Orderid { get; set; }

       
        public string? Menuitemid { get; set; }

     
        public string Itemname { get; set; } = string.Empty;

       
        public decimal Quantity { get; set; } 

       
        public decimal Unitprice { get; set; } 

      
        public decimal Totalprice { get; set; } 

        public decimal? DiscountAmount { get; set; } 

       
        public decimal? TaxAmount { get; set; } 

        public string? Modifierdetails { get; set; }

        
        public string? Itemstatus { get; set; } 

       
        public string? Notes { get; set; }

        public int OrgId { get; set; }
         
              

       
        public Orders? Order { get; set; }
    }
}

