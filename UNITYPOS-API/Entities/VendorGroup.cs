namespace UNITYPOS_API.Entities
{
    public class VendorGroup
    {
        public long Vendorgroupid { get; set; }
        public string Groupname { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int OrgId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
