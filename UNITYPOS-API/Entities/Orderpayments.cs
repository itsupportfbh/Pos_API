using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Orderpayments:CommonClass
    {

        [Key]
        public long PaymentId { get; set; }

        public long OrderId { get; set; }
               
        public string Paymenttype { get; set; } 

       
        public decimal Amount { get; set; }

        
        public string? Referenceno { get; set; }

       
        public string? Paymentstatus { get; set; } 

        public int OrgId { get; set; }
                 

        public Orders? Order { get; set; }
    }
}
