namespace UNITYPOS_API.Entities
{
    public class MoveTabledetails
    {
        public long Id { get; set; }
        public string JoinNo { get; set; } = string.Empty;
        public int TableId { get; set; }
        public int OrgId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
