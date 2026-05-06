namespace UNITYPOS_API.Entities.Master
{
    public class PaymodeMaster:CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }          // Cash / Card / UPI / Wallet
        public string Remarks { get; set; }
        public int OrgId { get; set; }
    }
}
