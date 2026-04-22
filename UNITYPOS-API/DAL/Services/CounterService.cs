using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class CounterService:ICounterService
    {
        private readonly IUnitOfWork _uow;
        public CounterService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }




        public IEnumerable<object> GetAllCounter(int orgId, int branchId)
        {
            var result = (from c in _uow.GenericRepository<Counter>().Table()
                          where c.IsActive == true
                                && c.IsDeleted == false
                                && c.OrgId == orgId
                                && c.BranchId == branchId
                          select new
                          {
                              id = c.Id,
                              name = c.Name,
                              code = c.Code,
                              branchId = c.BranchId,
                              orgId = c.OrgId,
                              isActive = c.IsActive
                          }).ToList();

            return result;
        }


        public IEnumerable<Object> GetCounterbyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<Counter>().Table()
                      where b.IsActive == true && b.IsDeleted == false && b.Id == id
                      select new
                      {
                          Id = b.Id,
                          Code = b.Code,
                          Name = b.Name,
                          Phone = b.Phone,
                          BranchId = b.BranchId,
                          Remarks = b.Remarks,
                          OrgId = b.OrgId,
                          IsActive = b.IsActive,
                          IsDeleted = b.IsDeleted,
                          CreatedBy = b.CreatedBy,
                          CreatedDate = b.CreatedDate,
                          UpdatedBy = b.UpdatedBy,
                          UpdatedDate = b.UpdatedDate

                      }).ToList();


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
            // 🔍 Duplicate check (exclude current record)
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

            // 🔍 Get existing record
            var existingCounter = _uow.GenericRepository<Counter>().Table()
                .FirstOrDefault(x =>
                    x.Id == counter.Id &&
                    x.OrgId == counter.OrgId &&
                    x.BranchId == counter.BranchId &&
                    x.IsDeleted == false);

            if (existingCounter != null)
            {
                // ✅ Update values
                existingCounter.Code = counter.Code;
                existingCounter.Name = counter.Name;
                existingCounter.Phone = counter.Phone;
                existingCounter.Remarks = counter.Remarks;
                existingCounter.IsActive = true;
                existingCounter.IsDeleted = false;

                existingCounter.UpdatedBy = counter.UpdatedBy;
                existingCounter.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Counter>().Update(existingCounter);
                _uow.Save();

                return existingCounter.Id.ToString();
            }

            return "0";
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
