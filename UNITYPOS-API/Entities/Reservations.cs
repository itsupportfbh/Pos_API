using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Reservations
    {
        public long Id { get; set; }
        public string ReservationNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeOnly Reservationtime { get; set; }
        public int ExpectedDuration { get; set; }
        public int Guestcount { get; set; }
        public string Specialrequests { get; set; }
        public string Bookingsource { get; set; }
        public int OrgId { get; set; }
        [NotMapped]
        public List<ReservationTablesMapping> TableIds {  get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        [NotMapped]
        public int EntityNo { get; set; }
        [NotMapped]
        public int branchId { get; set; }
    }
}
