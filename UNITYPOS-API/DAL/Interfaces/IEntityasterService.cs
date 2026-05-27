using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IEntityMasterService
    {
        public IEnumerable<EntityRoleRights> GetEntityMasterForRoleRights(int orgId, int roleId);
        public string SaveRolePermission(List<RolePermission> rolePermissions);
        public IEnumerable<object> GetRoleRightsByRoleId(int OrgId, int RoleId, int EntityNo);

    }
}
