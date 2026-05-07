using System.ComponentModel.DataAnnotations;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Vendor : CommonClass
    {
        public long Vendorid { get; set; }
        public string Vendorname { get; set; }
        public long? Vendorgroupid { get; set; }   
        public string Contactperson { get; set; }               
        public string Mobilenumber { get; set; }       
        public string Email { get; set; }        
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Gstnumber { get; set; }
        public string Bankname { get; set; }
        public string Accountnumber { get; set; }
        public string Ifsccode { get; set; }
        public string Status { get; set; }
        public int OrgId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
