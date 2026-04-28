namespace UNITYPOS_API.Entities.Master
{
    public class UserMaster:CommonClass
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Remarks { get; set; }
        public bool? IsAdmin { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? ContactNo { get; set; }
        public string? EmpCode { get; set; }
        public int? OrgId { get; set; }
        public string? Image { get; set; }
        public int? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public int? Age { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public int? City { get; set; }
        public int? State { get; set; }
        public int? Country { get; set; }
        public int? PostalCode { get; set; }
    }

    public class CreateUserMaster : CommonClass
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Remarks { get; set; }
        public bool? IsAdmin { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? ContactNo { get; set; }
        public string? EmpCode { get; set; }
        public int? OrgId { get; set; }
        public string? Image { get; set; }
        public int? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public int? Age { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public int? City { get; set; }
        public int? State { get; set; }
        public int? Country { get; set; }
        public int? PostalCode { get; set; }

        public List<UserBranchMapping>? UserBranchMapping { get; set; }
        public List<UserRoleMapping>? UserRoleMapping { get; set; }

    }
}
