using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class PrinterService:IPrinterService
    {

        private readonly IUnitOfWork _uow;
        public PrinterService(IUnitOfWork uow)
        {

            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }


        public IEnumerable<object> GetAll(int orgid, int branchid, int counterid, int terminalid)
        {
            var result = (from p in _uow.GenericRepository<Printer>().Table()
                          where p.IsDeleted == false
                                && p.OrgId == orgid
                                && (branchid == 0 || p.BranchId == branchid)
                                && (counterid == 0 || p.CounterId == counterid)
                                && (terminalid == 0 || p.TerminalId == terminalid)
                                
                          select new
                          {
                              id = p.Id,
                              orgId = p.OrgId,
                              branchId = p.BranchId,
                              counterId = p.CounterId,
                              terminalId = p.TerminalId,
                              code = p.Code,
                              name = p.Name,
                              remarks = p.Remarks,
                              isActive = p.IsActive
                          }).ToList();

            return result;
        }
        public IEnumerable<object> GetById(int id)
        {
            var result = (from p in _uow.GenericRepository<Printer>().Table()
                          where p.IsActive == true
                                && p.IsDeleted == false
                                && p.Id == id
                          select new
                          {
                              id = p.Id,
                              orgId = p.OrgId,
                              branchId = p.BranchId,
                              counterId = p.CounterId,
                              name = p.Name,
                              code = p.Code,
                              remarks = p.Remarks,
                              isActive = p.IsActive
                          }).ToList();

            return result;
        }

        public string Create(Printer printer)
        {
            int check = _uow.GenericRepository<Printer>().Table()
                .Count(p => p.Name.Trim().ToLower() == printer.Name.Trim().ToLower()
                         && p.OrgId == printer.OrgId
                         && p.BranchId == printer.BranchId
                         && p.CounterId == printer.CounterId
                         && p.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new Printer
            {
                OrgId = printer.OrgId,
                BranchId = printer.BranchId,
                CounterId = printer.CounterId,
                TerminalId = printer.TerminalId,
                Code = printer.Code,
                Name = printer.Name,
                Remarks = printer.Remarks,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = printer.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<Printer>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(Printer printer)
        {
            int check = _uow.GenericRepository<Printer>().Table()
                .Count(p => (p.Name.Trim().ToLower() == printer.Name.Trim().ToLower()
                          || p.Code.Trim().ToLower() == printer.Code.Trim().ToLower())
                         && p.Id != printer.Id
                         && p.OrgId == printer.OrgId
                         && p.BranchId == printer.BranchId
                         && p.CounterId == printer.CounterId
                         && p.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingPrinter = _uow.GenericRepository<Printer>().Table()
                .FirstOrDefault(x => x.Id == printer.Id
                                  && x.OrgId == printer.OrgId
                                  && x.IsDeleted == false);

            if (existingPrinter != null)
            {
                existingPrinter.OrgId = printer.OrgId;
                existingPrinter.BranchId = printer.BranchId;
                existingPrinter.CounterId = printer.CounterId;
                existingPrinter.Code = printer.Code;
                existingPrinter.Name = printer.Name;
                existingPrinter.Remarks = printer.Remarks;
                existingPrinter.IsActive = printer.IsActive;
                existingPrinter.IsDeleted = false;
                existingPrinter.UpdatedBy = printer.UpdatedBy;
                existingPrinter.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Printer>().Update(existingPrinter);
                _uow.Save();

                return Convert.ToString(existingPrinter.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<Printer>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<Printer>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<Printer>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsActive = isActive;
                _uow.GenericRepository<Printer>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }




    }
}
