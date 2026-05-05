using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Tenders:CommonClass
    {

        [Key]
        public long Tenderid { get; set; }

        public long Orderid { get; set; }

        [Required]
        [StringLength(30)]
        public string Paymenttype { get; set; } 
             
        public decimal Pamount { get; set; }
              
        public decimal? Tipamount { get; set; } = 0.00m;
               
        public decimal? Changeamount { get; set; } = 0.00m;

        [StringLength(100)]
        public string? Referenceno { get; set; }

        [StringLength(20)]
        public string? Cardtype { get; set; }

        [StringLength(20)]
        public string? Paymentstatus { get; set; } 

        public DateTime? Tendertime { get; set; }

        public int OrgId { get; set; }

       
        [ForeignKey(nameof(Orderid))]
        public Orders? Order { get; set; }

    }
}
