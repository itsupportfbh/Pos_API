using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IReservation
    {
        public IEnumerable<Object> GetAllReservation(int orgid);
        public IEnumerable<Object> GetReservationbyId(int id);
        public string Create(Reservations Reservation);
        public string Update(Reservations Reservation);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
