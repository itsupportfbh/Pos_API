using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IUserMasterService
    {
        public String Create(CreateUserMaster userMaster);
        public String Update(CreateUserMaster userMaster);
        public IEnumerable<Object> GetAllUsers(int OrgId);
        public UserMaster GetById(int Id);
        public string Delete(int Id);
        public string ActiveInActive(int Id, bool IsActive);

    }
}
