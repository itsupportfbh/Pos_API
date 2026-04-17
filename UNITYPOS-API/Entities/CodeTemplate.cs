namespace UNITYPOS_API.Entities
{
    public class CodeTemplate:CommonClass
    {
        public int Id { get; set; }
        public int EntityNo { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public int CurrentValue { get; set; }
        public string Suffix { get; set; }
        public int BranchId { get; set; }
        public bool IsMaster { get; set; }
    }
}
