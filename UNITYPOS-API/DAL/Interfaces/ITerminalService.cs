using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ITerminalService
    {
        public IEnumerable<Object> GetAllTerminal(int orgid, int branchid, int counterid);
        public IEnumerable<Object> GetTerminalbyId(int id);
        public string Create(Terminal terminal);
        public string Update(Terminal terminal);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
