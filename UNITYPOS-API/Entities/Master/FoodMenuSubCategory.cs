using System.ComponentModel.DataAnnotations.Schema;

namespace UNITYPOS_API.Entities.Master
{
    public class FoodMenuSubCategory : CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }        
        public int OrgId { get; set; }
        [NotMapped]
        public int EntityNo { get; set; }
        [NotMapped]
        public int BranchId { get; set; }
    }
}
