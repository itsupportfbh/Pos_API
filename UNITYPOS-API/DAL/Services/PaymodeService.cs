using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class PaymodeService : IPaymodeService
    {

        private readonly IUnitOfWork _uow;
        public PaymodeService(IUnitOfWork uow)
        {

            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public string Create(PaymodeMaster paymode)
        {
            int check = _uow.GenericRepository<PaymodeMaster>().Table()
                .Count(o => o.Type.ToLower() == paymode.Type.ToLower()
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new PaymodeMaster
            {
                Code        = paymode.Code,
                Type        = paymode.Type,
                OrgId       = paymode.OrgId,
                Remarks     = paymode.Remarks,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = paymode.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<PaymodeMaster>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(PaymodeMaster paymode)
        {
            int check = _uow.GenericRepository<PaymodeMaster>().Table()
                .Count(o => o.Type.ToLower() == paymode.Type.ToLower() && o.Id != paymode.Id
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var Existingpaymode = _uow.GenericRepository<PaymodeMaster>().Table().Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == paymode.Id).FirstOrDefault();

            if (Existingpaymode != null)
            {
                Existingpaymode.Code     = paymode.Code;
                Existingpaymode.Type     = paymode.Type;
                Existingpaymode.OrgId    = paymode.OrgId;
                Existingpaymode.Remarks  = paymode.Remarks;
                Existingpaymode.IsActive = true;
                Existingpaymode.IsDeleted = false;
                Existingpaymode.UpdatedBy = paymode.UpdatedBy;
                Existingpaymode.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<PaymodeMaster>().Update(Existingpaymode);
                _uow.Save();
            }
            else
            {
                return "";
            }

            return Convert.ToString(Existingpaymode.Id);
        }


        public IEnumerable<Object> GetAll(int orgid)
        {
            IEnumerable<Object> result = null;

            result = (from f in _uow.GenericRepository<PaymodeMaster>().Table()join
                      o in _uow.GenericRepository<Organization>().Table() on f.OrgId equals o.Id
                      where f.IsDeleted == false &&
                     (orgid == 0 || f.OrgId == 0 || f.OrgId == orgid) 

                      select new
                      {
                          Id = f.Id,
                          Code = f.Code,
                          Type = f.Type,
                          OrganizationId = f.OrgId,
                          IsActive = f.IsActive,
                          OrganizationName = o.Name
                          
                      })
                         .ToList();

            return result;
        }


        public PaymodeMaster GetById(int Id)
        {
            var result = _uow.GenericRepository<PaymodeMaster>()
                       .Table()
                       .Where(x => x.Id == Id && x.IsActive == true && x.IsDeleted == false)
                       .FirstOrDefault();

            return result;
        }

        public string Delete(int Id)
        {
            var result = _uow.GenericRepository<PaymodeMaster>().Table().Where(x => x.Id == Id).FirstOrDefault();
            if (result != null)
            {
                result.IsDeleted = true;
                _uow.GenericRepository<PaymodeMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        public string ActiveInActive(int Id, bool IsActive)
        {
            var result = _uow.GenericRepository<PaymodeMaster>().Table().Where(x => x.Id == Id).FirstOrDefault();
            if (result != null)
            {
                result.IsActive = IsActive;
                _uow.GenericRepository<PaymodeMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }


    }
}
