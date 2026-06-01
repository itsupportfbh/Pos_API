namespace UNITYPOS_API.Entities.Reports
{
    public class ReportPermission
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public int ReportId { get; set; }
        public int RoleId { get; set; }
        public bool CanView { get; set; }
        public bool CanPrint { get; set; }
        public bool ExportPdf { get; set; }
        public bool ExportExcel { get; set; }
    }
}
