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

                          join e in _uow.GenericRepository<EmployeeMaster>().Table()
                          on b.StewardId equals e.Id

                          where b.IsDeleted == false
                          && (orgid == 0 || b.OrgId == orgid)

                          select new
                          {
                              id = b.Id,
                              moveno = b.MoveNo,
                              organizationname = o.Name,
                              guestcount = b.GuestCount,
                              stewardid = b.StewardId,
                              stewardname = e.Name,
                              movereason = b.MoveReason
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
                          moveno = b.MoveNo,
                          guestcount = b.GuestCount,
                          stewardid = b.StewardId,
                          movereason = b.MoveReason,
                          status = b.IsActive,
                          TableIds = _uow
                          .GenericRepository<MoveTabledetails>()
                          .Table()
                          .Where(x =>
                              x.MoveNo == b.MoveNo
                              && x.IsDeleted == false)
                          .Select(x => new
                          {
                              fromTable = x.FromTable,
                              ToTable = x.ToTable,
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
                GuestCount = table.GuestCount,
                StewardId = table.StewardId,
                MoveReason = table.MoveReason,
                IsActive = table.IsActive,
                OrgId = table.OrgId,
                IsDeleted = false,
                CreatedBy = table.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<MoveTables>().Insert(entity);
            _uow.Save();

            if (table.TableDetails != null && table.TableDetails.Count > 0)
            {
                foreach (var tableId in table.TableDetails)
                {
                    var mapping = new MoveTabledetails
                    {
                        MoveNo = entity.MoveNo,
                        FromTable = tableId.FromTable,
                        ToTable = tableId.ToTable,
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
                existingtable.GuestCount = table.GuestCount;
                existingtable.StewardId = table.StewardId;
                existingtable.MoveReason = table.MoveReason;
                existingtable.IsActive = table.IsActive;
                existingtable.OrgId = table.OrgId;
                existingtable.IsDeleted = false;
                existingtable.UpdatedBy = table.UpdatedBy;
                existingtable.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<MoveTables>().Update(existingtable);

                if (table.TableDetails != null && table.TableDetails.Count > 0)
                {
                    foreach (var tableId in table.TableDetails)
                    {
                        var mapping = new MoveTabledetails
                        {
                            MoveNo = table.MoveNo,
                            FromTable = tableId.FromTable,
                            ToTable = tableId.ToTable,
                            OrgId = table.OrgId,
                            IsDeleted = false,
                            CreatedBy = table.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        _uow.GenericRepository<MoveTabledetails>()
                            .Insert(mapping);
                    }
                }

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
