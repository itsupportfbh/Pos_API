using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class RoleMasterService : IRoleMasterService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICodeTemplateService _codeTemplateService;

        public RoleMasterService(IUnitOfWork uow, ICodeTemplateService codeTemplateService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _codeTemplateService = codeTemplateService;
        }

        public IEnumerable<Object> GetAllRole(int orgid)
        {
            IEnumerable<Object> result = null;

            result = (from r in _uow.GenericRepository<RoleMaster>().Table()
                      join o in _uow.GenericRepository<Organization>().Table() on r.OrgId equals o.Id
                      where r.IsDeleted == false && o.IsDeleted == false && (orgid == 0 || r.OrgId == orgid)
                      select new
                      {
                          r.Id,
                          r.Code,
                          r.Name,
                          r.Remarks,
                          r.OrgId,
                          OrganizationName = o.Name,
                          r.IsActive,
                      }).ToList();

            return result;
        }

        public RoleMaster GetRoleById(int id)
        {
            var result = _uow.GenericRepository<RoleMaster>()
                .Table()
                .Where(x => x.Id == id && x.IsActive == true && x.IsDeleted == false)
                .FirstOrDefault();

            return result;
        }

        public string Create(RoleMaster roleMaster)
        {
            int check = _uow.GenericRepository<RoleMaster>().Table()
                .Count(r => r.Name.Trim().ToLower() == roleMaster.Name.Trim().ToLower()
                         && r.OrgId == roleMaster.OrgId
                         && r.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            string Code = _codeTemplateService.GetLatestCode(roleMaster.EntityNo, roleMaster.OrgId, 0);

            var entity = new RoleMaster
            {
                Code = Code,
                Name = roleMaster.Name,
                Remarks = roleMaster.Remarks,
                OrgId = roleMaster.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = roleMaster.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<RoleMaster>().Insert(entity);

            var codeTemplate = _uow.GenericRepository<CodeTemplate>().Table()
                               .FirstOrDefault(x => x.EntityNo == roleMaster.EntityNo
                                                 && x.OrgId == roleMaster.OrgId
                                                 && x.BranchId == 0
                                                 && x.IsMaster == true);

            if(codeTemplate != null)
            {
                codeTemplate.CurrentValue = codeTemplate.CurrentValue + 1;
                _uow.GenericRepository<CodeTemplate>().Update(codeTemplate);
            }

            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(RoleMaster roleMaster)
        {
            int check = _uow.GenericRepository<RoleMaster>().Table()
                .Count(r => r.Name.Trim().ToLower() == roleMaster.Name.Trim().ToLower()
                         && r.Id != roleMaster.Id
                         && r.OrgId == roleMaster.OrgId
                         && r.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingRole = _uow.GenericRepository<RoleMaster>().Table()
                .FirstOrDefault(x => x.Id == roleMaster.Id
                                  && x.OrgId == roleMaster.OrgId
                                  && x.IsDeleted == false);

            if (existingRole != null)
            {
                existingRole.Code = roleMaster.Code;
                existingRole.Name = roleMaster.Name;
                existingRole.Remarks = roleMaster.Remarks;
                existingRole.OrgId = roleMaster.OrgId;
                existingRole.IsActive = true;
                existingRole.IsDeleted = false;
                existingRole.UpdatedBy = roleMaster.UpdatedBy;
                existingRole.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<RoleMaster>().Update(existingRole);
                _uow.Save();
            }
            else
            {
                return "";
            }

            return Convert.ToString(existingRole.Id);
        }

        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<RoleMaster>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {
                result.IsDeleted = true;
                _uow.GenericRepository<RoleMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<RoleMaster>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {
                result.IsActive = isActive;
                _uow.GenericRepository<RoleMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }
    }
}
