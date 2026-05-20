using UNITYPOS_API.Controllers;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.Entities;


namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IEmployeeMasterService
    {

        public IEnumerable<object> GetAllEmployee(int orgId, string branchId);
        public EmployeeMaster GetEmployeebyId(int Id);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
        public string Create(EmployeeMaster employeeMaster);
        string Update(EmployeeMaster request);
    }
}
