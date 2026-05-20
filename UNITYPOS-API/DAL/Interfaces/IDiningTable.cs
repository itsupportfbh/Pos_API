using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IDiningTable
    {
        public IEnumerable<Object> GetAllDiningTable(int orgid);
        public IEnumerable<Object> GetDiningTablebyId(int id);
        public string Create(DiningTableMaster Table);
        public string Update(DiningTableMaster Table);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
