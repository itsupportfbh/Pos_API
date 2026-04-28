using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class RoleMasterService : IRoleMasterService
    {
        private readonly IUnitOfWork _uow;

        public RoleMasterService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<Object> GetAllRole(int orgid)
        {
            IEnumerable<Object> result = null;

            result = (from r in _uow.GenericRepository<RoleMaster>().Table()
                      where r.IsDeleted == false && r.OrgId == orgid
                      select new
                      {
                          r.Id,
                          r.Code,
                          r.Name,
                          r.Remarks,
                          r.OrgId,
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

            var entity = new RoleMaster
            {
                Code = roleMaster.Code,
                Name = roleMaster.Name,
                Remarks = roleMaster.Remarks,
                OrgId = roleMaster.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = roleMaster.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<RoleMaster>().Insert(entity);
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
