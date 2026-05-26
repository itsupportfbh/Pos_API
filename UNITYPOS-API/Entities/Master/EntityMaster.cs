namespace UNITYPOS_API.Entities.Master
{
    public class EntityMaster: CommonClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EntityNo { get; set; }
        public bool IsMaster { get; set; }
        
    }

    public class EntityRoleRights
    {
        public int OrgId { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public int EntityNo { get; set; }
        public string EntityName { get; set; }
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool View { get; set; }
        public bool Download { get; set; }
        public bool Print { get; set; }
        public bool ActiveInActive { get; set; }
    }
}
