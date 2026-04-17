namespace UNITYPOS_API.ViewModel
{
    public class OrganizationDTO
    {

        public int Id { get; set; }
        public string? Code { get; set; } 
        public string? Name { get; set; } 
        public string? GSTNo { get; set; }
        public string? RegistrationNo { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }

        public string? ContactPerson { get; set; }
        public string? ContactMobileNo { get; set; }
        public string? ContactEmail { get; set; }

        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public int? City { get; set; }
        public int? State { get; set; }
        public int? PostalCode { get; set; }
        public int? Country { get; set; }
        public string? Image { get; set; }
        public string? ThemeColor { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; } = true;
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }


        //
        public string? CountryName { get; set; }

    }
}
