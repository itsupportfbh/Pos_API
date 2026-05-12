namespace UNITYPOS_API.Entities
{
    public class ComboMenu
    {

        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        public decimal Price { get; set; }

        public int OrgId { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public List<ComboMenuItem> ComboMenuItems { get; set; }

    }
}
