namespace UNITYPOS_API.Entities.Master
{
    public class EntityMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EntityNo { get; set; }
        public bool IsMaster { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
