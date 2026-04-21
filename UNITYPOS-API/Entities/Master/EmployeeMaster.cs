namespace UNITYPOS_API.Entities.Master
{
    public class EmployeeMaster : CommonClass
    {
        public int Id { get; set; }

        public string Code { get; set; }              // EMP001
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }

        public string Designation { get; set; }       // Cashier / Waiter / Chef / Manager
        public string Department { get; set; }        // Service / Kitchen / Billing
        public DateTime? DateOfJoining { get; set; }

        public int? BranchId { get; set; }
               
              
        public string Gender { get; set; }
        public string AddressLine1 { get; set; }
        public string IdProofNo { get; set; }         // Aadhaar / PAN / etc.
        public string Remarks { get; set; }
        public int OrgId { get; set; }


    }
}
