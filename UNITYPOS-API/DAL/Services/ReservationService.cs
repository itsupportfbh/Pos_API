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
                              id = b.ReservationNo,
                              organizationname = o.Name,
                              Customernam = b.CustomerName,
                              name = b.CustomerMobile,
                              code = b.ReservationDate,
                              price = b.Reservationtime,
                              categoryId = b.Guestcount,
                              categoryname = b.Status,
                          }).ToList();

            return result;
        }

        public IEnumerable<Object> GetReservationbyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<FoodMenu>().Table()
                      where b.IsDeleted == false && b.Id == id
                      select new
                      {
                          id = b.Id,
                          name = b.Name,
                          code = b.Code,
                          categoryId = b.CategoryId,
                          price = b.Price,
                          isactive = b.IsActive,
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
                Status = Reservation.Status,
                Specialrequests = Reservation.Specialrequests,
                Bookingsource = Reservation.Bookingsource,
                OrgId = Reservation.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = Reservation.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<Reservations>().Insert(entity);
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
                existingRes.ExpectedDuration  = reservations.ExpectedDuration;
                existingRes.Reservationtime = reservations.Reservationtime;
                existingRes.Guestcount = reservations.Guestcount;
                existingRes.Status = reservations.Status;
                existingRes.Specialrequests = reservations.Specialrequests;
                existingRes.Bookingsource = reservations.Bookingsource;
                existingRes.OrgId = reservations.OrgId;
                existingRes.IsActive = true;
                existingRes.IsDeleted = false;
                existingRes.UpdatedBy = reservations.UpdatedBy;
                existingRes.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Reservations>().Update(existingRes);
                _uow.Save();

                return Convert.ToString(existingRes.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<Reservations>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<Reservations>().Update(result);
                _uow.Save();
            }
             
            return Convert.ToString(result?.Id ?? 0);


        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<Reservations>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsActive = isActive;
                _uow.GenericRepository<Reservations>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }
    }
}
