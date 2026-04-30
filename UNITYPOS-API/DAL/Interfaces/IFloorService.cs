using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IFloorService
    {
        public string Create(FloorMaster floor);
        public string Update(FloorMaster floor);
        public IEnumerable<Object> GetAllFloor(int orgid, int branchid);
        public FloorMaster GetById(int Id);
        public string Delete(int Id);
        public string ActiveInActive(int Id, bool IsActive);
    }
}
