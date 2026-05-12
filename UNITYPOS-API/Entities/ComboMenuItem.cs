using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class ComboMenuItem
    {
        public int Id { get; set; }

        public int ComboMenuId { get; set; }
        public int FoodMenuId { get; set; }

        public decimal Qty { get; set; }
        public decimal Price { get; set; }

        public int OrgId { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ComboMenu? ComboMenu { get; set; }

        public virtual FoodMenu? FoodMenu { get; set; }

    }
}
