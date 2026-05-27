using System.ComponentModel.DataAnnotations;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class EmployeeMaster : CommonClass
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        [StringLength(20)]
        public string MobileNo { get; set; }

        public string EmailId { get; set; }

        public int? DesignationId { get; set; }

        public int? DepartmentId { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public int? BranchId { get; set; }

        public string Gender { get; set; }

        public string? AddressLine1 { get; set; }

        public string? IdProofNo { get; set; }

        public string Remarks { get; set; }

        public int OrgId { get; set; }
    }

}

