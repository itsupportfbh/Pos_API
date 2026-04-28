namespace UNITYPOS_API.Entities.Master
{
    public class UserBranchMapping:CommonClass
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
    }
}
