using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Tips:CommonClass
    {

        [Key]
        public long Tipid { get; set; }

        public long Orderid { get; set; }

        public long? Tenderid { get; set; }

        [StringLength(50)]
        public string? Employeeid { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Tipamount { get; set; }

        [StringLength(20)]
        public string? Tiptype { get; set; } = "MANUAL";

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Adjustmentamount { get; set; } = 0.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Finaltipamount { get; set; }

        [StringLength(255)]
        public string? Adjustmentreason { get; set; }

        [StringLength(20)]
        public string? Status { get; set; } = "ACTIVE";

        public int OrgId { get; set; }
             

        public Orders? Order { get; set; }

      
        public Tenders? Tender { get; set; }

    }
}
