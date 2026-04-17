namespace UNITYPOS_API.Entities
{
    public class Shift:CommonClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TerminalId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string?  Status { get; set; }
        public int BranchId { get; set; }

    }
}
