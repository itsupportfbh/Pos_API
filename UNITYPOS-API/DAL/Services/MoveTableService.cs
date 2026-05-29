using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class MoveTableService : IMoveTable
    {
        private readonly IUnitOfWork _uow;
        public MoveTableService(IUnitOfWork uow)
        { 
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<object> GetAllMoveTable(int orgid)
        {
            var result = (from b in _uow.GenericRepository<MoveTables>().Table()

                          join o in _uow.GenericRepository<Organization>().Table()
                          on b.OrgId equals o.Id

                          join d in _uow.GenericRepository<DiningTableMaster>().Table()
                          on b.FromTable equals d.Id

                          join e in _uow.GenericRepository<EmployeeMaster>().Table()
                          on b.StewardId equals e.Id

                          where b.IsDeleted == false
                          && (orgid == 0 || b.OrgId == orgid)

                          select new
                          {
                              id = b.Id,
                              joinno = b.MoveNo,
                              organizationname = o.Name,
                              primarytable = b.FromTable,
                              primaryname = d.Name,
                              guestcount = b.GuestCount,
                              stewardid = b.StewardId,
                              stewardname = e.Name,
                              notes = b.Notes,
                              status = b.IsActive
                          }).ToList();

            return result;
        }

        public IEnumerable<Object> GetMoveTablebyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<MoveTables>().Table()
                      where b.IsDeleted == false && b.Id == id
                      select new
                      {
                          Id = b.Id,
                          joinno = b.MoveNo,
                          primarytable = b.FromTable,
                          guestcount = b.GuestCount,
                          stewardid = b.StewardId,
                          notes = b.Notes,
                          status = b.IsActive,
                          TableIds = _uow
                          .GenericRepository<MoveTabledetails>()
                          .Table()
                          .Where(x =>
                              x.JoinNo == b.MoveNo
                              && x.IsDeleted == false)
                          .Select(x => new
                          {
                              tableId = x.TableId
                          }).ToList()
                      }).ToList();


            return result;
        }


        public string Create(MoveTables table)
        {
            int check = _uow.GenericRepository<MoveTables>().Table()
                .Count(b => b.MoveNo.Trim().ToLower() == table.MoveNo.Trim().ToLower()
                         && b.OrgId == table.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new MoveTables
            {
                MoveNo = table.MoveNo,
                FromTable = table.FromTable,
                GuestCount = table.GuestCount,
                StewardId = table.StewardId,
                Notes = table.Notes,
                IsActive = table.IsActive,
                OrgId = table.OrgId,
                IsDeleted = false,
                CreatedBy = table.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<MoveTables>().Insert(entity);
            _uow.Save();

            if (table.TableIds != null && table.TableIds.Count > 0)
            {
                foreach (var tableId in table.TableIds)
                {
                    var mapping = new MoveTabledetails
                    {
                        JoinNo = entity.MoveNo,
                        TableId = tableId.TableId,
                        OrgId = table.OrgId,
                        IsDeleted = false,
                        CreatedBy = table.CreatedBy,
                        CreatedDate = DateTime.Now
                    };

                    _uow.GenericRepository<MoveTabledetails>()
                        .Insert(mapping);
                }
            }

            var codeTemplate = _uow.GenericRepository<CodeTemplate>().Table()
                               .FirstOrDefault(x => x.EntityNo == table.EntityNo
                                                 && x.OrgId == table.OrgId
                                                 && x.BranchId == table.branchId
                                                 );

            if (codeTemplate != null)
            {
                codeTemplate.CurrentValue = codeTemplate.CurrentValue + 1;
                _uow.GenericRepository<CodeTemplate>().Update(codeTemplate);
            }

            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(MoveTables table)
        {
            int check = _uow.GenericRepository<MoveTables>().Table()
                .Count(b => (b.MoveNo.Trim().ToLower() == table.MoveNo.Trim().ToLower())
                         && b.Id != table.Id
                         && b.OrgId == table.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingtable = _uow.GenericRepository<MoveTables>().Table()
                .FirstOrDefault(x => x.Id == table.Id
                                  && x.OrgId == table.OrgId
                                  && x.IsDeleted == false);

            if (existingtable != null)
            {
                existingtable.FromTable = table.FromTable;
                existingtable.GuestCount = table.GuestCount;
                existingtable.StewardId = table.StewardId;
                existingtable.Notes = table.Notes;
                existingtable.IsActive = table.IsActive;
                existingtable.OrgId = table.OrgId;
                existingtable.IsDeleted = false;
                existingtable.UpdatedBy = table.UpdatedBy;
                existingtable.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<MoveTables>().Update(existingtable);
                _uow.Save();

                return Convert.ToString(existingtable.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<MoveTables>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<MoveTables>().Update(result);
                _uow.Save();
            }

            var mappings = _uow.GenericRepository<MoveTabledetails>().Table().Where(x => x.Id == id).ToList();
            if (mappings != null && mappings.Count > 0)
            {
                foreach (var item in mappings)
                {
                    item.IsDeleted = true;

                    _uow.GenericRepository<MoveTabledetails>()
                        .Update(item);
                }
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<MoveTables>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                //result.Status = "isActive";
                _uow.GenericRepository<MoveTables>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }
    }
}
