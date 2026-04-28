using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IUserBranchMappingService
    {
        public String Create(List<UserBranchMapping> userBranchMapping);
        public String Update(List<UserBranchMapping> userBranchMapping);
    }
}
