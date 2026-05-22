using System.ComponentModel.DataAnnotations.Schema;

namespace UNITYPOS_API.Entities.Master
{
    public class Terminal:CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; } 
        public string Name { get; set; }
        public int BranchId { get; set; }
        public int CounterId { get; set; }
        public string? Remarks { get; set; }
        public string  DeviceName { get; set; }
        public int OrgId { get; set; }
        [NotMapped]
        public int EntityNo { get; set; }
    }
}
