namespace UNITYPOS_API.Entities.Reports
{
    public class ReportFilter
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ControlType { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsShow { get; set; }

    }
}
