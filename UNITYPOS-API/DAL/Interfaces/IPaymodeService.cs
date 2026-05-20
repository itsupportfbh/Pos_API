using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IPaymodeService
    {
        public string Create(PaymodeMaster paymode);
        public string Update(PaymodeMaster paymode);
        public IEnumerable<Object> GetAll(int orgid);
        public PaymodeMaster GetById(int Id);
        public string Delete(int Id);
        public string ActiveInActive(int Id, bool IsActive);
    }
}
