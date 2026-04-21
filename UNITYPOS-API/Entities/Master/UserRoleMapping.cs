namespace UNITYPOS_API.Entities.Master
{
    public class UserRoleMapping:CommonClass
    {

        public int Id { get; set; }
        public int UserId {  get; set; }
        public int RoleId { get; set; }

    }
}
