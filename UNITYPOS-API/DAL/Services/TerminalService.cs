using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class TerminalService : ITerminalService
    {


        private readonly IUnitOfWork _uow;
        public TerminalService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<Object> GetAllTerminal(int orgid, int branchid, int counterid)
        {
            IEnumerable<Object> result = null;

            result = (from t in _uow.GenericRepository<Terminal>().Table()
                      join b in _uow.GenericRepository<Branch>().Table()
                          on t.BranchId equals b.Id
                      join c in _uow.GenericRepository<Counter>().Table()
                           on t.CounterId equals c.Id
                      where t.IsDeleted == false
                            && t.OrgId == orgid
                            && (branchid==0||t.BranchId == branchid)
                            && (counterid == 0 || t.CounterId == counterid)
                            && t.BranchId == branchid
                            && (counterid == 0 || c.Id == counterid)
                      select new
                      {
                          id = t.Id,
                          name = t.Name,
                          code = t.Code,
                          branchid = t.BranchId,
                          branchname = b.Name,
                          counterid = c.Id,
                          countername = c.Name,
                          isactive = t.IsActive,
                      }).ToList();


            return result;
        }
        public IEnumerable<Object> GetTerminalbyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<Terminal>().Table()
                      where b.IsActive == true && b.IsDeleted == false && b.Id == id
                      select new
                      {
                          id = b.Id,
                          name = b.Name,
                          code = b.Code,
                          branchId = b.BranchId,
                          counterId = b.CounterId,
                          deviceName = b.DeviceName,
                          isactive = b.IsActive,
                      }).ToList();


            return result;
        }

        public string Create(Terminal terminal)
        {

            string hostName = Environment.MachineName;
            int check = _uow.GenericRepository<Terminal>().Table()
                .Count(c => c.Name.ToLower() == terminal.Name.ToLower()
                         && c.OrgId == terminal.OrgId
                         && c.BranchId == terminal.BranchId
                         && c.CounterId == terminal.CounterId
                         && c.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new Terminal
            {

                OrgId = terminal.OrgId,
                BranchId = terminal.BranchId,
                CounterId = terminal.CounterId,
                DeviceName = terminal.DeviceName,
                Code = terminal.Code,
                Name = terminal.Name,
                Remarks = terminal.Remarks,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = terminal.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<Terminal>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(Terminal terminal)
        {
            int check = _uow.GenericRepository<Terminal>().Table()
                .Count(b => (b.Name.Trim().ToLower() == terminal.Name.Trim().ToLower()
                          || b.Code.Trim().ToLower() == terminal.Code.Trim().ToLower())
                         && b.Id != terminal.Id
                         && b.OrgId == terminal.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingTerminal = _uow.GenericRepository<Terminal>().Table()
                .FirstOrDefault(x => x.Id == terminal.Id
                                  && x.OrgId == terminal.OrgId
                                  && x.IsDeleted == false);

            if (existingTerminal != null)
            {
                string hostName = Environment.MachineName;

                existingTerminal.OrgId = terminal.OrgId;
                existingTerminal.BranchId = terminal.BranchId;
                existingTerminal.CounterId = terminal.CounterId;
                existingTerminal.DeviceName = terminal.DeviceName;
                existingTerminal.Code = terminal.Code;
                existingTerminal.Name = terminal.Name;
                existingTerminal.Remarks = terminal.Remarks;
                existingTerminal.IsActive = true;
                existingTerminal.IsDeleted = false;
                existingTerminal.UpdatedBy = terminal.UpdatedBy;
                existingTerminal.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Terminal>().Update(existingTerminal);
                _uow.Save();

                return Convert.ToString(existingTerminal.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<Terminal>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<Terminal>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<Terminal>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsActive = isActive;
                _uow.GenericRepository<Terminal>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }


    }
}
