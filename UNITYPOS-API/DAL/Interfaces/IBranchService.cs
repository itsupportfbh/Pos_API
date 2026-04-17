namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IBranchService
    {
        public IEnumerable<Object> GetAllBranch(int orgid);
    }
}
