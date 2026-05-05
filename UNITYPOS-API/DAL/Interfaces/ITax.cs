using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ITax
    {
        public IEnumerable<Object> GetAllTax(int orgid);
        public Tax GetTaxbyId(int id);
        public string Create(Tax Menu);
        public string Update(Tax Menu);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
