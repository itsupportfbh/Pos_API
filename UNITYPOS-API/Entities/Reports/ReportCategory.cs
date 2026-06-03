using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities.Reports
{
    public class ReportCategory : CommonClass
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
