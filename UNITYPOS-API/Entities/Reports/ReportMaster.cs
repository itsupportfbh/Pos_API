using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities.Reports
{
    public class ReportMaster : CommonClass
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public int CategoryId { get; set; }
        public string ReportCode { get; set; } = string.Empty;
        public string ReportName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string StoredProcedure { get; set; } = string.Empty;
        public string TemplateName { get; set; } = string.Empty;
        public string TemplatePath { get; set; } = string.Empty;
        public string? ReportType { get; set; }
        public string? ViewerType { get; set; }
        public decimal? PaperWidth { get; set; }
        public decimal? PaperHeight { get; set; }
        public string? Orientation { get; set; }
        public bool IsThermal { get; set; }
        public bool IsLandscape { get; set; }
        public int DisplayOrder { get; set; }
        public string? Icon { get; set; }
    }
}
