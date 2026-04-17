namespace UNITYPOS_API.Entities
{
    public class UserRoleMapping:CommonClass
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId {  get; set; }
        public int RoleId { get; set; }

    }
}
