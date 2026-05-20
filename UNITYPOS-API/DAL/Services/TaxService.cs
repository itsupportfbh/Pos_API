using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class TaxService : ITax
    {
        private readonly IUnitOfWork _uow;
        private readonly ICodeTemplateService _codeTemplateService;

        public TaxService(IUnitOfWork uow, ICodeTemplateService codeTemplateService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _codeTemplateService = codeTemplateService;
        }

        public IEnumerable<Object> GetAllTax(int orgid)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<Tax>().Table()
                      join o in _uow.GenericRepository<Organization>().Table()
                         on b.OrgId equals o.Id
                      where b.IsDeleted == false && (orgid == 0 || b.OrgId == orgid)
                      select new
                      {
                          b.Id,
                          OrganizationName = o.Name,
                          b.Name,
                          b.Code,
                          b.Percentage,
                          b.IsActive,
                      }).ToList();


            return result;
        }
        public Tax GetTaxbyId(int id)
        {
            var result = _uow.GenericRepository<Tax>()
                     .Table()
                     .Where(x => x.Id == id && x.IsActive == true && x.IsDeleted == false)
                     .FirstOrDefault();

            return result;
        }


        public string Create(Tax tax)
        {
            int check = _uow.GenericRepository<Tax>().Table()
                .Count(b => b.Name.Trim().ToLower() == tax.Name.Trim().ToLower()
                         && b.OrgId == tax.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            string Code = _codeTemplateService.GetLatestCode(tax.EntityNo, tax.OrgId, 0);

            var entity = new Tax
            {
                Code = Code,
                Name = tax.Name,
                Percentage = tax.Percentage,
                OrgId = tax.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = tax.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<Tax>().Insert(entity);

            var codeTemplate = _uow.GenericRepository<CodeTemplate>().Table()
                             .FirstOrDefault(x => x.EntityNo == tax.EntityNo
                                               && x.OrgId == tax.OrgId
                                               && x.BranchId == 0
                                               && x.IsMaster == true);

            if (codeTemplate != null)
            {
                codeTemplate.CurrentValue = codeTemplate.CurrentValue + 1;
                _uow.GenericRepository<CodeTemplate>().Update(codeTemplate);
            }

            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(Tax tax)
        {
            int check = _uow.GenericRepository<Tax>().Table()
                .Count(b => (b.Name.Trim().ToLower() == tax.Name.Trim().ToLower()
                          || b.Code.Trim().ToLower() == tax.Code.Trim().ToLower())
                         && b.Id != tax.Id
                         && b.OrgId == tax.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingtax = _uow.GenericRepository<Tax>().Table()
                .FirstOrDefault(x => x.Id == tax.Id
                                  && x.OrgId == tax.OrgId
                                  && x.IsDeleted == false);

            if (existingtax != null)
            {
                existingtax.Code = tax.Code;
                existingtax.Name = tax.Name;
                existingtax.Percentage = tax.Percentage;
                existingtax.OrgId = tax.OrgId;
                existingtax.IsActive = true;
                existingtax.IsDeleted = false;
                existingtax.UpdatedBy = tax.UpdatedBy;
                existingtax.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Tax>().Update(existingtax);
                _uow.Save();

                return Convert.ToString(existingtax.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<Tax>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<Tax>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<Tax>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsActive = isActive;
                _uow.GenericRepository<Tax>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }
    }
}
