using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IOrganizationService
    {
        public String Create(Organization organization);
        public String Update(Organization organization);
        public IEnumerable<Object> GetAllOrganization();
        public Organization GetById(int Id);
        public string Delete(int Id);
        public string ActiveInActive(int Id, bool IsActive);
        public String CreateUpdateOrganizationConfig(OrganizationConfig organizationconfig);
        public OrganizationConfig GetOrganizationConfigByOrgId(int OrgId);

    }
}
