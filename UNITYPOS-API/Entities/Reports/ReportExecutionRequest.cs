using System.Text.Json;

namespace UNITYPOS_API.Entities.Reports
{
    public class ReportExecutionRequest
    {
        public int OrgId { get; set; }
        public int RoleId { get; set; }
        public int ReportId { get; set; }
        public Dictionary<string, JsonElement>? Filters { get; set; }
    }
}
