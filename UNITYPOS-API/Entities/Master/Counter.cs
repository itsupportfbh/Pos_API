namespace UNITYPOS_API.Entities.Master
{
    public class Counter:CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int BranchId { get; set; }
        public string? Remarks { get; set; }
        public int OrgId { get; set; }

    }
}
