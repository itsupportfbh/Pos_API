namespace UNITYPOS_API.Entities.Master
{
    public class SubMenu
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public int EntityNo { get; set; }
        public int DisplayOrder { get; set; }
        public string Route { get; set; }
        public string Remarks { get; set; }
        public int Menuscope { get; set; }
        public string MenuIcon { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
