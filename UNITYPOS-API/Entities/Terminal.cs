namespace UNITYPOS_API.Entities
{
    public class Terminal:CommonClass
    {
        public int Id { get; set; }
        public string? Code { get; set; } 
        public string? Name { get; set; }
        public int BranchId { get; set; }
        public int CounterId { get; set; }
        public string? Remarks { get; set; }
        public int? DeviceId { get; set; }
       


    }
}
