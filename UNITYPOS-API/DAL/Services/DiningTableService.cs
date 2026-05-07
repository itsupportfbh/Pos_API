using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.DAL.Interfaces;

namespace UNITYPOS_API.DAL.Services
{
    public class DiningTableService : IDiningTable
    {
        private readonly IUnitOfWork _uow;
        public DiningTableService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<Object> GetAllDiningTable(int orgid)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<DiningTableMaster>().Table()                      
                      join o in _uow.GenericRepository<Organization>().Table()
                        on b.OrgId equals o.Id
                      where b.IsDeleted == false && (orgid == 0 || b.OrgId == orgid)
                      select new
                      {
                          id = b.Id,
                          organizationname = o.Name,
                          name = b.Name,
                          code = b.Code,
                          seatingsize = b.SeatingSize,
                          reservable = b.IsReservable,
                          occupied = b.IsOccupied,
                          remarks = b.Remarks,
                      }).ToList();


            return result;
        }
        public IEnumerable<Object> GetDiningTablebyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<DiningTableMaster>().Table()
                      where b.IsDeleted == false && b.Id == id
                      select new
                      {
                          id = b.Id,
                          name = b.Name,
                          code = b.Code,
                          seatingsize = b.SeatingSize,
                          reservable = b.IsReservable,
                      }).ToList();


            return result;
        }


        public string Create(DiningTableMaster Table)
        {
            int check = _uow.GenericRepository<DiningTableMaster>().Table()
                .Count(b => b.Name.Trim().ToLower() == Table.Name.Trim().ToLower()
                         && b.OrgId == Table.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new DiningTableMaster
            {
                Code = Table.Code,
                Name = Table.Name,
                SeatingSize = Table.SeatingSize,
                IsAvailable = Table.IsAvailable,
                IsReservable = Table.IsReservable,
                IsOccupied = Table.IsOccupied,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = Table.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<DiningTableMaster>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(DiningTableMaster Table)
        {
            int check = _uow.GenericRepository<DiningTableMaster>().Table()
                .Count(b => (b.Name.Trim().ToLower() == Table.Name.Trim().ToLower()
                          || b.Code.Trim().ToLower() == Table.Code.Trim().ToLower())
                         && b.Id != Table.Id
                         && b.OrgId == Table.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingMenu = _uow.GenericRepository<DiningTableMaster>().Table()
                .FirstOrDefault(x => x.Id == Table.Id
                                  && x.OrgId == Table.OrgId
                                  && x.IsDeleted == false);

            if (existingMenu != null)
            {
                existingMenu.Code = Table.Code;
                existingMenu.Name = Table.Name;
                existingMenu.SeatingSize = Table.SeatingSize;
                existingMenu.IsOccupied = Table.IsOccupied;
                existingMenu.OrgId = Table.OrgId;
                existingMenu.IsActive = true;
                existingMenu.IsDeleted = false;
                existingMenu.UpdatedBy = Table.UpdatedBy;
                existingMenu.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<DiningTableMaster>().Update(existingMenu);
                _uow.Save();

                return Convert.ToString(existingMenu.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<DiningTableMaster>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<DiningTableMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<DiningTableMaster>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsActive = isActive;
                _uow.GenericRepository<DiningTableMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }
    }
}
