using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ICounterService
    {

        public IEnumerable<object> GetAllCounter(int orgId, int branchId);
        public IEnumerable<Object> GetCounterbyId(int id);
        public string Create(Counter counter);
        public string Update(Counter counter);
    }
}
