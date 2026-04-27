using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ICustomerService
    {
        public IEnumerable<object> GetAllCustomer(int orgid);
        public CustomerMaster GetCustomerbyId(int Id);
        public string Create(CustomerMaster customer);
        public string Update(CustomerMaster customer);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
