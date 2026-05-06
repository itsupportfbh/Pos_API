using System.Security.Cryptography;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class FloorService : IFloorService
    {

        private readonly IUnitOfWork _uow;
        public FloorService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public string Create(FloorMaster floor)
        {
            int check = _uow.GenericRepository<FloorMaster>().Table()
                .Count(o => o.Name.ToLower() == floor.Name.ToLower()
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new FloorMaster
            {
                Code = floor.Code,
                Name = floor.Name,
                BranchId = floor.BranchId,
                OrgId = floor.OrgId,
                Remarks = floor.Remarks,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = floor.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<FloorMaster>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(FloorMaster floor)
        {
            int check = _uow.GenericRepository<FloorMaster>().Table()
                .Count(o => o.Name.ToLower() == floor.Name.ToLower() && o.Id != floor.Id
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var ExistingFloor = _uow.GenericRepository<FloorMaster>().Table().Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == floor.Id).FirstOrDefault();

            if (ExistingFloor != null)
            {
                ExistingFloor.Code = floor.Code;
                ExistingFloor.Name = floor.Name;
                ExistingFloor.BranchId = floor.BranchId;
                ExistingFloor.OrgId = floor.OrgId;
                ExistingFloor.Remarks = floor.Remarks;
                ExistingFloor.IsActive = true;
                ExistingFloor.IsDeleted = false;
                ExistingFloor.UpdatedBy = floor.UpdatedBy;
                ExistingFloor.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<FloorMaster>().Update(ExistingFloor);
                _uow.Save();
            }
            else
            {
                return "";
            }

            return Convert.ToString(ExistingFloor.Id);
        }


        public IEnumerable<Object> GetAllFloor(int orgid, int branchid)
        {
            IEnumerable<Object> result = null;

            result = (from f in _uow.GenericRepository<FloorMaster>().Table()
                      join
                      b in _uow.GenericRepository<Branch>().Table() on f.BranchId equals b.Id
                      join
                      o in _uow.GenericRepository<Organization>().Table() on f.OrgId equals o.Id
                      where f.IsDeleted == false &&
                     (orgid == 0 || f.OrgId == orgid) &&
                     (branchid == 0 || f.BranchId == branchid)

                      select new
                      {
                          Id = f.Id,
                          Code = f.Code,
                          Name = f.Name,
                          BranchId = f.BranchId,
                          OrganizationId = f.OrgId,
                          IsActive = f.IsActive,
                          BranchName = b.Name,
                          OrganizationName = o.Name
                      })
                         .ToList();

            return result;
        }


        public FloorMaster GetById(int Id)
        {
            var result = _uow.GenericRepository<FloorMaster>()
                       .Table()
                       .Where(x => x.Id == Id && x.IsActive == true && x.IsDeleted == false)
                       .FirstOrDefault();

            return result;
        }

        public string Delete(int Id)
        {
            var result = _uow.GenericRepository<FloorMaster>().Table().Where(x => x.Id == Id).FirstOrDefault();
            if (result != null)
            {
                result.IsDeleted = true;
                _uow.GenericRepository<FloorMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        public string ActiveInActive(int Id, bool IsActive)
        {
            var result = _uow.GenericRepository<FloorMaster>().Table().Where(x => x.Id == Id).FirstOrDefault();
            if (result != null)
            {
                result.IsActive = IsActive;
                _uow.GenericRepository<FloorMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }
    }
}
