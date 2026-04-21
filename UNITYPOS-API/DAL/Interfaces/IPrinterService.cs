using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IPrinterService
    {
        public IEnumerable<object> GetAll(int orgid, int branchid, int counterid, int terminalid);
        public IEnumerable<object> GetById(int id);
        public string Create(Printer printer);
        public string Update(Printer printer);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
