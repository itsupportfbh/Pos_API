using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class ReservationService : IReservation
    {
        private readonly IUnitOfWork _uow;
        public ReservationService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<object> GetAllReservation(int orgid)
        {
            var result = (from b in _uow.GenericRepository<Reservations>().Table()

                          join o in _uow.GenericRepository<Organization>().Table()
                          on b.OrgId equals o.Id

                          where b.IsDeleted == false
                          && (orgid == 0 || b.OrgId == orgid)

                          select new
                          {
                              Id = b.Id,
                              ReservationNo = b.ReservationNo,
                              organizationname = o.Name,
                              CustomerName = b.CustomerName,
                              CustomerMobile = b.CustomerMobile,
                              ReservationDate = b.ReservationDate,
                              Reservationtime = b.Reservationtime,
                              Guestcount = b.Guestcount,
                              TableName = string.Join(", ",
                                           (from rm in _uow.GenericRepository<ReservationTablesMapping>().Table()
                                            join dt in _uow.GenericRepository<DiningTableMaster>().Table()
                                            on rm.TableId equals dt.Id
                                            where rm.ReservationId == b.Id
                                            && rm.IsDeleted == false
                                            && dt.IsDeleted == false

                                            select dt.Name
                                            ).ToList())
                          }).ToList();

            return result;
        }

        public IEnumerable<Object> GetReservationbyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<Reservations>().Table()
                      where b.IsDeleted == false && b.Id == id
                      select new
                      {
                          Id = b.Id,
                          ReservationNo = b.ReservationNo,
                          CustomerName = b.CustomerName,
                          CustomerMobile = b.CustomerMobile,
                          CustomerEmail = b.CustomerEmail,
                          ReservationDate = b.ReservationDate,
                          Reservationtime = b.Reservationtime,
                          Guestcount = b.Guestcount,
                          TableIds = _uow
                          .GenericRepository<ReservationTablesMapping>()
                          .Table()
                          .Where(x =>
                              x.ReservationId == b.Id
                              && x.IsDeleted == false)
                          .Select(x => new
                          {
                              tableId = x.TableId
                          }).ToList()
                      }).ToList();


            return result;
        }


        public string Create(Reservations Reservation)
        {
            int check = _uow.GenericRepository<Reservations>().Table()
                .Count(b => b.ReservationNo.Trim().ToLower() == Reservation.ReservationNo.Trim().ToLower()
                         && b.OrgId == Reservation.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new Reservations
            {
                ReservationNo = Reservation.ReservationNo,
                CustomerName = Reservation.CustomerName,
                CustomerMobile = Reservation.CustomerMobile,
                CustomerEmail = Reservation.CustomerEmail,
                ReservationDate = Reservation.ReservationDate,
                Reservationtime = Reservation.Reservationtime,
                ExpectedDuration = Reservation.ExpectedDuration,
                Guestcount = Reservation.Guestcount,
                Specialrequests = Reservation.Specialrequests,
                Bookingsource = Reservation.Bookingsource,
                OrgId = Reservation.OrgId,
                IsDeleted = false,
                CreatedBy = Reservation.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<Reservations>().Insert(entity);
            _uow.Save();

            if (Reservation.TableIds != null && Reservation.TableIds.Count > 0)
            {
                foreach (var tableId in Reservation.TableIds)
                {
                    var mapping = new ReservationTablesMapping
                    {
                        ReservationId = entity.Id,
                        TableId = tableId.TableId,
                        OrgId = Reservation.OrgId,
                        IsDeleted = false,
                        CreatedBy = Reservation.CreatedBy,
                        CreatedDate = DateTime.Now
                    };

                    _uow.GenericRepository<ReservationTablesMapping>()
                        .Insert(mapping);

                    var table = _uow.GenericRepository<DiningTableMaster>().Table().FirstOrDefault(x => x.Id == tableId.TableId
                      && x.OrgId == Reservation.OrgId && x.BranchId == Reservation.branchId);

                    if (table != null)
                    {
                        table.IsOccupied = true;

                        _uow.GenericRepository<DiningTableMaster>()
                            .Update(table);
                    }
                }
            }

            var codeTemplate = _uow.GenericRepository<CodeTemplate>().Table()
                               .FirstOrDefault(x => x.EntityNo == Reservation.EntityNo
                                                 && x.OrgId == Reservation.OrgId
                                                 && x.BranchId == Reservation.branchId
                                                 );

            if (codeTemplate != null)
            {
                codeTemplate.CurrentValue = codeTemplate.CurrentValue + 1;
                _uow.GenericRepository<CodeTemplate>().Update(codeTemplate);
            }

            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(Reservations reservations)
        {
            int check = _uow.GenericRepository<Reservations>().Table()
                .Count(b => (b.ReservationNo.Trim().ToLower() == reservations.ReservationNo.Trim().ToLower())
                         && b.Id != reservations.Id
                         && b.OrgId == reservations.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingRes = _uow.GenericRepository<Reservations>().Table()
                .FirstOrDefault(x => x.Id == reservations.Id
                                  && x.OrgId == reservations.OrgId
                                  && x.IsDeleted == false);

            if (existingRes != null)
            {
                existingRes.CustomerName = reservations.CustomerName;
                existingRes.CustomerMobile = reservations.CustomerMobile;
                existingRes.CustomerEmail = reservations.CustomerEmail;
                existingRes.ReservationDate = reservations.ReservationDate;
                existingRes.ExpectedDuration = reservations.ExpectedDuration;
                existingRes.Reservationtime = reservations.Reservationtime;
                existingRes.Guestcount = reservations.Guestcount;
                existingRes.Specialrequests = reservations.Specialrequests;
                existingRes.Bookingsource = reservations.Bookingsource;
                existingRes.OrgId = reservations.OrgId;
                existingRes.IsDeleted = false;
                existingRes.UpdatedBy = reservations.UpdatedBy;
                existingRes.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Reservations>().Update(existingRes);
                _uow.Save();

                var existingMappings = _uow.GenericRepository<ReservationTablesMapping>()
        .Table()
        .Where(x => x.ReservationId == existingRes.Id
                 && x.OrgId == reservations.OrgId
                 && x.IsDeleted == false)
        .ToList();

                var selectedTableIds = reservations.TableIds
                    .Select(x => x.TableId)
                    .ToList();


                foreach (var map in existingMappings.Where(x => !selectedTableIds.Contains(x.TableId)))
                {
                    map.IsDeleted = true;
                    _uow.GenericRepository<ReservationTablesMapping>().Update(map);

                    var table = _uow.GenericRepository<DiningTableMaster>()
                        .Table()
                        .FirstOrDefault(x => x.Id == map.TableId);

                    if (table != null)
                    {
                        table.IsOccupied = false;
                        _uow.GenericRepository<DiningTableMaster>().Update(table);
                    }
                }

                foreach (var tableId in selectedTableIds.Where(id => !existingMappings.Any(x => x.TableId == id)))
                {
                    _uow.GenericRepository<ReservationTablesMapping>().Insert(
                        new ReservationTablesMapping
                        {
                            ReservationId = existingRes.Id,
                            TableId = tableId,
                            OrgId = reservations.OrgId,
                            IsDeleted = false,
                            CreatedBy = reservations.UpdatedBy,
                            CreatedDate = DateTime.Now
                        });

                    var table = _uow.GenericRepository<DiningTableMaster>()
                        .Table()
                        .FirstOrDefault(x => x.Id == tableId);

                    if (table != null)
                    {
                        table.IsOccupied = true;
                        _uow.GenericRepository<DiningTableMaster>().Update(table);
                    }
                }
                return Convert.ToString(existingRes.Id);
            }

            _uow.Save();

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<Reservations>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<Reservations>().Update(result);
            }

            var mappings = _uow.GenericRepository<ReservationTablesMapping>().Table().Where(x => x.ReservationId == id).ToList();
            if (mappings != null && mappings.Count > 0)
            {
                foreach (var item in mappings)
                {
                    item.IsDeleted = true;

                    _uow.GenericRepository<ReservationTablesMapping>()
                        .Update(item);

                    var table = _uow.GenericRepository<DiningTableMaster>().Table().FirstOrDefault(x => x.Id == item.TableId
                      && x.OrgId == item.OrgId);

                    if (table != null)
                    {
                        table.IsOccupied = false;

                        _uow.GenericRepository<DiningTableMaster>()
                            .Update(table);
                    }
                }
            }

            _uow.Save();

            return Convert.ToString(result?.Id ?? 0);


        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<Reservations>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                //result.Status = "isActive";
                _uow.GenericRepository<Reservations>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }
    }
}
