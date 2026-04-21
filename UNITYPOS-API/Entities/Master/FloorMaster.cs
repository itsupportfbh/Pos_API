namespace UNITYPOS_API.Entities.Master
{
    public class FloorMaster:CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int BranchId { get; set; }
        public int OrgId { get; set; }
    }
}
