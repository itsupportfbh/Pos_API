using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNITYPOS_API.Entities
{
    public class Dayend
    {
        public long Dayendid { get; set; }
        public DateTime Businessdate { get; set; }
        public decimal Totalsales { get; set; }
        public int Totalorders { get; set; }
        public decimal Totalcashsales { get; set; }      
        public decimal Totalcardsales { get; set; }
        public decimal Totalupisales { get; set; }
        public decimal Totalothersales { get; set; }
        public decimal Totaltax { get; set; }
        public decimal Totaldiscount { get; set; }
        public decimal Totalrefund { get; set; }
        public decimal Totaltips { get; set; }
        public decimal Openingcash { get; set; }
        public decimal Closingcash { get; set; }
        public decimal Expectedcash { get; set; }
        public decimal Cashdifference { get; set; }
        public string Status { get; set; }
        public DateTime? GeneratedDate { get; set; }
        public string Generatedby { get; set; }
        public int OrgId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
