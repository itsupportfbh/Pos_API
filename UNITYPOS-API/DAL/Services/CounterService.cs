using System.Diagnostics.Metrics;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class CounterService : ICounterService
    {
        private readonly IUnitOfWork _uow;
        public CounterService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }




        public IEnumerable<object> GetAllCounter(int orgId, List<int> branchIds)
        {
            var result =
                (from c in _uow.GenericRepository<Counter>().Table()
                 join b in _uow.GenericRepository<Branch>().Table()
                    on c.BranchId equals b.Id
                 where c.IsDeleted == false
                       && c.OrgId == orgId
                       && (!branchIds.Any() || branchIds.Contains(c.BranchId))
                 select new
                 {
                     Id = c.Id,
                     Code = c.Code,
                     Name = c.Name,
                     Phone = c.Phone,
                     BranchId = c.BranchId,
                     BranchName = b.Name,
                     OrgId = c.OrgId,
                     IsActive = c.IsActive,
                     IsDeleted = c.IsDeleted,
                     Remarks = c.Remarks
                 }).ToList();

            return result;
        }


        public Counter GetCounterbyId(int Id)
        {
            var result = _uow.GenericRepository<Counter>()
                      .Table()
                      .Where(x => x.Id == Id && x.IsActive == true && x.IsDeleted == false)
                      .FirstOrDefault();

            return result;
        }

        public string Create(Counter counter)
        {
            int check = _uow.GenericRepository<Counter>().Table()
                .Count(c => c.Name.ToLower() == counter.Name.ToLower()
                         && c.OrgId == counter.OrgId
                         && c.BranchId == counter.BranchId
                         && c.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new Counter
            {
                Code = counter.Code,
                Name = counter.Name,
                Phone = counter.Phone,
                Remarks = counter.Remarks,
                OrgId = counter.OrgId,
                BranchId = counter.BranchId,
               
                IsActive = true,
                IsDeleted = false,
                CreatedBy = counter.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<Counter>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }


        public string Update(Counter counter)
        {
            int check = _uow.GenericRepository<Counter>().Table()
                .Count(x =>
                    x.Id != counter.Id &&
                    x.OrgId == counter.OrgId &&
                    x.BranchId == counter.BranchId &&
                    x.IsDeleted == false &&
                    (
                        x.Name.Trim().ToLower() == counter.Name.Trim().ToLower()
                    ));

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingCounter = _uow.GenericRepository<Counter>().Table()
                .FirstOrDefault(x =>
                    x.Id == counter.Id &&
                    x.IsDeleted == false);

            if (existingCounter != null)
            {
                existingCounter.Code = counter.Code;
                existingCounter.Name = counter.Name;
                existingCounter.Phone = counter.Phone;
                existingCounter.Remarks = counter.Remarks;
                existingCounter.BranchId = counter.BranchId;
                existingCounter.IsActive = true;
                existingCounter.IsDeleted = false;
                existingCounter.UpdatedBy = counter.UpdatedBy;
                existingCounter.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Counter>().Update(existingCounter);
                _uow.Save();

            }
            else

            {
                return "";
            }

            return Convert.ToString(existingCounter.Id);
        }

        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<Counter>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<Counter>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }


        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<Counter>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsActive = isActive;
                _uow.GenericRepository<Counter>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }
    }
}
