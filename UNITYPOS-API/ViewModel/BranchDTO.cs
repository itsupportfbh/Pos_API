using UNITYPOS_API.Entities;

namespace UNITYPOS_API.ViewModel
{
    public class BranchDTO:CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? Email { get; set; }


        public string? ContactPerson { get; set; }
        public string? ContactMobileNo { get; set; }
        public string? ContactEmail { get; set; }

        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public int? City { get; set; }
        public int? State { get; set; }
        public int? PostalCode { get; set; }
        public int? Country { get; set; }
        public string? Remarks { get; set; }
    }
}
