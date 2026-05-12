using System.ComponentModel.DataAnnotations.Schema;

namespace UNITYPOS_API.Entities.Master
{
    public class Tax:CommonClass
    {

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public int OrgId { get; set; }
        [NotMapped]
        public int EntityNo { get; set; }
    }
}
