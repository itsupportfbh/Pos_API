namespace UNITYPOS_API.Entities
{
    public class UserBranchMapping:CommonClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
    }
}
