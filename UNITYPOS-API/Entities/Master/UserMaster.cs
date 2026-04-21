namespace UNITYPOS_API.Entities.Master
{
    public class UserMaster:CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int ContactNo { get; set; }
        public string EmpCode { get; set; }

        public int OrgId { get; set; }



    }
}
