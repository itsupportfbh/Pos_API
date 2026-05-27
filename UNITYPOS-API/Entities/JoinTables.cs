using System.ComponentModel.DataAnnotations.Schema;

namespace UNITYPOS_API.Entities
{
    public class JoinTables
    {
        public long Id { get; set; }
        public string JoinNo {  get; set; }
        public int PrimaryTable {  get; set; }
        public int GuestCount { get; set; }
        public int StewardId { get; set; }
        public string Notes { get; set; }
        public bool? IsActive { get; set; } = true;
        public int OrgId { get; set; }
        [NotMapped]
        public List<JoinTabledetails> TableIds { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        [NotMapped]
        public int EntityNo { get; set; }
        [NotMapped]
        public int branchId { get; set; }
    }
}
