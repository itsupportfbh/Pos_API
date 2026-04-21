namespace UNITYPOS_API.Entities.Master
{
    public class CustomerMaster : CommonClass
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string AddressLine1 { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string? Pincode { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MemberNo { get; set; }
        public decimal OpeningBalance { get; set; }
        public bool IsMember { get; set; }
        public string Remarks { get; set; }
        public int OrgId { get; set; }

    }
}
