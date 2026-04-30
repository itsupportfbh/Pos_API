using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ICounterService
    {

        public IEnumerable<object> GetAllCounter(int orgId, string branchId);
        public Counter GetCounterbyId(int id);
        public string Create(Counter counter);
        public string Update(Counter counter);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
