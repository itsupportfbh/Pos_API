using System.ComponentModel.DataAnnotations;

namespace UNITYPOS_API.Entities.DBLog
{
    public class DBAudit
    {
        [Key]
        public int AuditRowId { get; set; }
        public string AuditId { get; set; }
        public DateTime AuditDate { get; set; }
        public string? TableName { get; set; }
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public string? OldData { get; set; }
        public string? NewData { get; set; }
        public string? ChangedColumns { get; set; }
    }
}
