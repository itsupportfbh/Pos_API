using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IJoinTables
    {
        public IEnumerable<Object> GetAllJoinTable(int orgid);
        public IEnumerable<Object> GetJoinTablebyId(int id);
        public string Create(JoinTables Table);
        public string Update(JoinTables Table);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
