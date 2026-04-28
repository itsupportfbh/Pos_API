using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IRoleMasterService
    {
        public IEnumerable<Object> GetAllRole(int orgid);
        public RoleMaster GetRoleById(int id);
        public string Create(RoleMaster roleMaster);
        public string Update(RoleMaster roleMaster);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
