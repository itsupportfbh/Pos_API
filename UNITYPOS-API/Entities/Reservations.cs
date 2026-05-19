using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Reservations : CommonClass
    {
        public int Id { get; set; }
        public string ReservationNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeOnly Reservationtime { get; set; }
        public int ExpectedDuration { get; set; }
        public int Guestcount { get; set; }
        public string Status { get; set; }
        public string Specialrequests { get; set; }
        public string Bookingsource { get; set; }
        public int OrgId { get; set; }
    }
}
