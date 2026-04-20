using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IBranchService
    {
        public IEnumerable<Object> GetAllBranch(int orgid);
        public IEnumerable<Object> GetBranchbyId(int id);
        public string Create(Branch branch);
        public string Update(Branch branch);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
