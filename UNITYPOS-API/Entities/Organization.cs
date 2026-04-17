namespace UNITYPOS_API.Entities
{
    public class Organization
    {

        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
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
        public string? Remarks { get; set; }
        public bool IsActive { get; set; } = true;
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
