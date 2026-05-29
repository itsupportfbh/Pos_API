using System.ComponentModel.DataAnnotations.Schema;

namespace UNITYPOS_API.Entities
{
    public class MoveTables
    {
        public long Id { get; set; }
        public string MoveNo { get; set; }
        public int FromTable { get; set; }
        public int GuestCount { get; set; }
        public int StewardId { get; set; }
        public string Notes { get; set; }
        public bool? IsActive { get; set; } = true;
        public int OrgId { get; set; }
        [NotMapped]
        public List<MoveTabledetails> TableIds { get; set; }
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
