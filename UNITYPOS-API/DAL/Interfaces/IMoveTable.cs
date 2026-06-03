using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IMoveTable
    {
        public IEnumerable<Object> GetAllMoveTable(int orgid);
        public IEnumerable<Object> GetMoveTablebyId(int id);
        public string Create(MoveTables Table);
        public string Update(MoveTables Table);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
 