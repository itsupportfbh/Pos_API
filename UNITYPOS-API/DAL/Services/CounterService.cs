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
                          id = b.Id,
                          name = b.Name,
                          code = b.Code,
                          isactive = b.IsActive,
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
            var existingcounter = _uow.GenericRepository<Counter>().Table()
               .FirstOrDefault(x =>
                   x.OrgId == counter.OrgId &&
                   x.BranchId == counter.BranchId &&
                   x.Id == counter.Id &&
                   x.IsDeleted == false &&
                   (x.Code.Trim().ToLower() == counter.Code.Trim().ToLower()
                    || x.Name.Trim().ToLower() == counter.Name.Trim().ToLower()));
            return Convert.ToString(existingcounter.Id);

        }



    }
}
