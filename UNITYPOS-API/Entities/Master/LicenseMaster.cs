namespace UNITYPOS_API.Entities.Master
{
    public class LicenseMaster:CommonClass
    {

        public int Id { get; set; }
        public string LicenseKey { get; set; } 
        public int BranchId { get; set; }
        public int CounterId { get; set; }
        public int TerminalId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int OrgId { get; set; }
    }
}
