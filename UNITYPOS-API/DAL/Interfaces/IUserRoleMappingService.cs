using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IUserRoleMappingService
    {
        public String Create(List<UserRoleMapping> userRoleMapping);
        public String Update(List<UserRoleMapping> userRoleMapping);
    }
}
